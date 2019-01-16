using System;
using System.Collections.Generic;
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
                    else if (tokens.Current == "system")
                    {
                        if(tokens.Next == "class")
                        {
                            tokens.MoveNext();
                            AppendUntilReachesSameLevel(tokens, serverBuilder);
                        }
                        else if(tokens.Next == "void")
                        {
                            tokens.MoveNext();
                            AppendUntilReachesSameLevel(tokens, serverBuilder);
                        }
                        else
                        {
                            tokens.MoveNext();

                            AppendUntilReachesSameLevel(tokens, serverBuilder, clientBuilder, true);
                        }
                    }
                }
            } while (tokens.MoveNext());
        }

        public void AppendUntilReachesSameLevel(TokenReader token, StringBuilder builderA, StringBuilder builderB = null, bool dontFillB = false, string toFill = "")
        {            
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
                            builderB.AppendLine(token.Current);

                            //toFill
                            if (foundFirst && token.Current == "{" && level == 1)
                            {
                                builderB.AppendLine(toFill);
                            }
                        }
                    }
                    else
                    {
                        builderB.AppendLine(token.Current);
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
        }

        


    }
}
