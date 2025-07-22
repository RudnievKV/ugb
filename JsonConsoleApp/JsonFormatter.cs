using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonConsoleApp
{
    public static class JsonFormatter
    {
        private enum ContainerType { Object, Array }

        private class ContainerContext
        {
            public ContainerType Type { get; set; }
            public bool IsEmpty { get; set; } = true;
        }

        public static string PrettyPrintJson(string json)
        {
            bool inString = false; // inside double quote ""
            bool inEscapeChar = false;
            int indentLevel = 0;
            var stack = new Stack<ContainerContext>();
            var output = new StringBuilder();

            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];

                if (inString)
                {
                    output.Append(c);
                    if (inEscapeChar)
                    {
                        inEscapeChar = false;
                    }
                    else if (c == '"')
                    {
                        inString = false;
                    }
                    else if (c == '\\')
                    {
                        inEscapeChar = true;
                    }
                }
                else
                {
                    if (char.IsWhiteSpace(c)) continue;


                    if (stack.Count > 0)
                    {
                        var currentContainer = stack.Peek();
                        if (currentContainer.IsEmpty && c != ',' && c != '}' && c != ']')
                        {
                            currentContainer.IsEmpty = false;
                        }
                    }

                    switch (c)
                    {
                        case '{':
                        case '[':
                            output.Append(c);
                            var containerType = c == '{' ? ContainerType.Object : ContainerType.Array;
                            stack.Push(new ContainerContext { Type = containerType });
                            indentLevel++;

                            int j = i + 1;
                            while (j < json.Length && char.IsWhiteSpace(json[j])) j++;
                            if (j < json.Length)
                            {
                                char next = json[j];
                                if ((containerType == ContainerType.Object && next == '}') ||
                                    (containerType == ContainerType.Array && next == ']')) // Empty container - no newline
                                {

                                }
                                else
                                {
                                    output.AppendLine();
                                    AppendIndent(output, indentLevel);
                                }
                            }
                            break;
                        case '}':
                        case ']':
                            if (stack.Count == 0) break;
                            var currentContainer = stack.Pop();
                            indentLevel--;

                            if (currentContainer.IsEmpty)
                            {
                                output.Append(c);
                            }
                            else
                            {
                                output.AppendLine();
                                AppendIndent(output, indentLevel);
                                output.Append(c);
                            }
                            break;
                        case ':':
                            output.Append(": ");
                            break;
                        case ',':
                            output.Append(c);
                            output.AppendLine();
                            AppendIndent(output, indentLevel);
                            break;
                        case '"':
                            inString = true;
                            output.Append(c);
                            break;
                        default:
                            output.Append(c);
                            break;
                    }
                }
            }

            return output.ToString();
        }

        private static void AppendIndent(StringBuilder sb, int level)
        {
            sb.Append(' ', level * 2);
        }
    }
}
