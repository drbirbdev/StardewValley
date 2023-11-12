using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BirbCore;
using BirbShared.APIs;
using StardewModdingAPI;

namespace BirbShared.Token
{
    public class TokenClassParser
    {
        private readonly IMod Mod;
        private readonly object Tokens;
        private readonly IContentPatcherApi Api;
        private readonly TokenClass TokenClass;

        public TokenClassParser(IMod mod, object tokens)
        {
            this.Mod = mod;
            this.Tokens = tokens;
            this.Api = mod.Helper.ModRegistry.GetApi<IContentPatcherApi>("Pathoschild.ContentPatcher");
            if (this.Api == null)
            {
                Log.Trace("Content Patcher is not enabled, so will skip parsing");
                return;
            }

            this.TokenClass = this.ParseClass();
        }

        private TokenClass ParseClass()
        {
            foreach (object attr in this.Tokens.GetType().GetCustomAttributes(false))
            {
                if (attr is TokenClass tokenClass)
                {
                    return tokenClass;
                }
            }
            Log.Error("Mod is attempting to parse token file, but is not using TokenClass attribute.  Please reach out to mod author to enable Token Class.");
            return null;
        }

        public bool IsEnabled()
        {
            return this.TokenClass != null;
        }

        public bool ParseTokens()
        {
            if (!this.IsEnabled())
            {
                return false;
            }

            this.ParseAllAttributes();

            return true;
        }

        private void ParseAllAttributes()
        {
            foreach (MethodInfo methodInfo in this.Tokens.GetType().GetMethods())
            {
                foreach(TokenMethod attr in methodInfo.GetCustomAttributes(typeof(TokenMethod), false))
                {
                    if (methodInfo.GetParameters().Length != 0)
                    {
                        Log.Warn("Token Attribute Method cannot use parameters");
                        continue;
                    }
                    if (methodInfo.ReturnType != typeof(IEnumerable<string>))
                    {
                        Log.Warn("Token Attribute Method must return an Ienumerable<string>");
                        continue;
                    }

                    Api.RegisterToken(this.Mod.ModManifest, methodInfo.Name, () =>
                    {
                        return methodInfo.Invoke(this.Tokens, Array.Empty<object>());
                    });
                }
            }
        }
    }
}
