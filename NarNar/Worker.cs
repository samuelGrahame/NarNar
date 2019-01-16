using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarNar
{
    public class Worker
    {

        public void Compile(string source)
        {
            var clientBuilder = new StringBuilder();
            var serverBuilder = new StringBuilder();

            var tokens = new TokenReader(source);

            do
            {
                if (tokens.Current == "system" || tokens.Current == "client" || tokens.Current == "server")
                {
                    // begin of stuff.
                    if(tokens.Current == "client")
                    {
                        // if class then just add to client...
                        tokens.MoveNext();
                        AppendUntilReachesSameLevel(tokens, clientBuilder);
                    }
                    else if (tokens.Current == "server")
                    {
                        if (tokens.NextNonWhite == "class")
                        {
                            tokens.MoveNext();
                            AppendUntilReachesSameLevel(tokens, serverBuilder, clientBuilder);
                        }
                        else if (tokens.NextNonWhite == "void")
                        {
                            tokens.MoveNext();
                            AppendUntilReachesSameLevel(tokens, serverBuilder);
                        }
                        else
                        {
                            tokens.MoveNext();

                            AppendUntilReachesSameLevel(tokens, serverBuilder, clientBuilder, true, "// INSERT SOME SERVER SIDE FUNC");
                        }
                    }
                    else if (tokens.Current == "system")
                    {
                        tokens.MoveNext();
                        AppendUntilReachesSameLevel(tokens, serverBuilder, clientBuilder);
                    }
                }
            } while (tokens.MoveNext());


            File.WriteAllText("client.cs", clientBuilder.ToString());
            File.WriteAllText("server.cs", serverBuilder.ToString());
        }

        public void AppendUntilReachesSameLevel(TokenReader token, StringBuilder builderA, StringBuilder builderB = null, bool dontFillB = false, string toFill = "")
        {
            if(builderA.Length != 0)
                builderA.AppendLine();

            if(builderB != null && builderB.Length != 0)
            {
                builderB.AppendLine();
            }

            if (string.IsNullOrWhiteSpace(token.Current))
                token.MoveNext();
            
            int level = 0;
            bool foundFirst = false;
            for (; ; )
            {
                if (token.Current == "{")
                {
                    level++;
                    foundFirst = true;
                }
                else if(token.Current == "}")
                {
                    level--;
                }

                builderA.Append(token.Current);
                if(builderB != null)
                {
                    if (dontFillB)
                    {
                        if(!foundFirst || (foundFirst && token.Current == "{" && level == 1) || (foundFirst && token.Current == "}" && level == 0))
                        {
                            builderB.Append(token.Current);

                            //toFill
                            if (foundFirst && token.Current == "{" && level == 1)
                            {
                                builderB.AppendLine();
                                builderB.Append(toFill);
                                builderB.AppendLine();
                            }
                        }
                    }
                    else
                    {
                        builderB.Append(token.Current);
                    }
                }               
                
                if(foundFirst && level == 0)
                {                    
                    token.MoveNext();
                    break;
                }

                if(!token.MoveNext())
                {
                    break;
                }
            }

            builderA.AppendLine();
            if (builderB != null)
            {
                builderB.AppendLine();
            }
        }

        


    }
}
