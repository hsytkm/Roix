﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン: 16.0.0.0
//  
//     このファイルへの変更は、正しくない動作の原因になる可能性があり、
//     コードが再生成されると失われます。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Roix.SourceGenerator
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class CodeTemplate : CodeTemplateBase
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("// <auto-generated>\r\n// THIS (.cs) FILE IS GENERATED BY UnitGenerator. DO NOT CHA" +
                    "NGE IT.\r\n// </auto-generated>\r\n#nullable enable\r\nusing System;\r\n\r\n");

    string GetProperties(System.Collections.Generic.IEnumerable<string> items, string format = "")
    {
        if (!string.IsNullOrWhiteSpace(format))
            return string.Join(", ", items.Select(x => string.Format(format, x)));

        return string.Join(", ", items);
    }

    string GetNames(string format = "")
    {
        return GetProperties(Properties.Select(p => p.Name), format);
    }

    string GetLowerNames(string format = "")
    {
        return GetProperties(Properties.Select(p => p.Name.ToLower()), format);
    }
    
    string GetInTypeIfNecessary(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax typeSyntax)
    {
        var type = typeSyntax.ToString();
        switch (type)
        {
            case "int":
            case "double":
                return type;
            default:
                return "in " + type;
        }
    }

    string GetTypeAndLowerNames(string format = "")
    {
        if (!string.IsNullOrWhiteSpace(format))
            return string.Join(", ", Properties.Select(p => string.Format(format, p.Type, p.Name.ToLower())));

        return string.Join(", ", Properties.Select(p => GetInTypeIfNecessary(p.Type) + " " + p.Name.ToLower()));
    }

    string GetToString()
    {
        return string.Join(", ", Properties.Select(p => p.Name + " = {" + p.Name + "}"));
    }
    
    string GetToStringWithFormat()
    {
        return string.Join(", ", Properties.Select(p => p.Name + " = {" + p.Name + ".ToString(format, formatProvider)}"));
    }
    
    string GetRoixSizeStructName()
    {
        return HasFlag(RoixStructGeneratorOptions.TypeInt) ? "RoixIntSize" : "RoixSize";
    }
    
    string GetRoixPointStructName()
    {
        return HasFlag(RoixStructGeneratorOptions.TypeInt) ? "RoixIntPoint" : "RoixPoint";
    }
    
    string GetRoixBuiltInType()
    {
        return HasFlag(RoixStructGeneratorOptions.TypeInt) ? "int" : "double";
    }
    
    string GetRoixNameWithoutInt()
    {
        return HasFlag(RoixStructGeneratorOptions.TypeInt) ? Name.Replace("Int", "") : Name;
    }

    string GetOperatorString(ArithmeticOperators ope)
    {
        if (ope == ArithmeticOperators.Add) return " + ";
        if (ope == ArithmeticOperators.Subtract) return " - ";
        if (ope == ArithmeticOperators.Multiply) return " * ";
        if (ope == ArithmeticOperators.Divide) return " / ";
        return "";
    }

    string GetOperate2Value(ArithmeticOperators ope, string name1, string name2)
    {
        var os = GetOperatorString(ope);
        return string.Join(", ", Properties.Select(p => name1 + "." + p.Name + os + name2 + "." + p.Name));
    }

    string GetRatioProperty(int index)
    {
        return ((index & 1) == 0) ? "X" : "Y";
    }
    
    string GetOperatorRoixAndRatio(ArithmeticOperators ope, string name1, string name2)
    {
        var os = GetOperatorString(ope);
        var roixs = Properties.Select(p => name1 + "." + p.Name).ToArray();

        var opes = new string[roixs.Length];
        for (int i = 0; i < roixs.Length; ++i)
        {
            opes[i] = roixs[i] + os + (name2 + "." + GetRatioProperty(i));
        }
        return string.Join(", ", opes);
    }
    
    string GetOperate1Value(ArithmeticOperators ope, string name1, string value2)
    {
        var os = GetOperatorString(ope);
        return string.Join(", ", Properties.Select(p => name1 + "." + p.Name + os + value2));
    }
    

            this.Write("\r\n");
 if (!string.IsNullOrEmpty(Namespace)) { 
            this.Write("namespace ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            this.Write("\r\n");
 } 
            this.Write("{\r\n    readonly partial struct ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" : IEquatable<");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(">, IFormattable\r\n    {\r\n        private readonly SourceValues _values;\r\n\r\n");
 foreach (var prop in Properties) { 
            this.Write("        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(prop.Type));
            this.Write(" ");
            this.Write(this.ToStringHelper.ToStringWithCulture(prop.Name));
            this.Write(" => this._values.");
            this.Write(this.ToStringHelper.ToStringWithCulture(prop.Name));
            this.Write(";\r\n");
 } 
            this.Write("\r\n");
 if (!IsConstructorDeclared) { 
            this.Write("        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write("(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeAndLowerNames()));
            this.Write(")\r\n        {\r\n            this._values = new(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetLowerNames()));
            this.Write(");\r\n  ");
 if (HasFlag(RoixStructGeneratorOptions.Validate)) { 
            this.Write(" \r\n            this.Validate(this);\r\n  ");
 } 
            this.Write("        }\r\n");
 } 
 if (HasFlag(RoixStructGeneratorOptions.Validate)) { 
            this.Write("        // RoixStructGeneratorOptions.Validate\r\n        private partial void Vali" +
                    "date(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value);\r\n");
 } 
            this.Write("        public void Deconstruct(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeAndLowerNames("out {0} {1}")));
            this.Write(") => (");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetLowerNames()));
            this.Write(") = (");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetNames()));
            this.Write(");\r\n\r\n        public bool Equals(");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" other) => this == other;\r\n        public override bool Equals(object? obj) => (o" +
                    "bj is ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" other) && Equals(other);\r\n        public override int GetHashCode() => HashCode." +
                    "Combine(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetNames()));
            this.Write(");\r\n        public static bool operator ==(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" left, in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" right) => (");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetNames("left.{0}")));
            this.Write(") == (");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetNames("right.{0}")));
            this.Write(");\r\n        public static bool operator !=(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" left, in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" right) => !(left == right);\r\n\r\n        public override string ToString() => $\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" {{ ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetToString()));
            this.Write(" }}\";\r\n        public string ToString(string? format, IFormatProvider? formatProv" +
                    "ider) => $\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" {{ ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetToStringWithFormat()));
            this.Write(" }}\";\r\n\r\n        public static ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" Zero { get; } = default;\r\n        public bool IsZero => this == Zero;\r\n        p" +
                    "ublic bool IsNotZero => !this.IsZero;\r\n");
 if (HasFlag(RoixStructGeneratorOptions.Rect)) { 
            this.Write("        // RoixStructGeneratorOptions.Rect\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixBuiltInType()));
            this.Write(" X => Location.X;\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixBuiltInType()));
            this.Write(" Y => Location.Y;\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixBuiltInType()));
            this.Write(" Width => Size.Width;\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixBuiltInType()));
            this.Write(" Height => Size.Height;\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixBuiltInType()));
            this.Write(" Left => Location.X;\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixBuiltInType()));
            this.Write(" Right => Location.X + Size.Width;\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixBuiltInType()));
            this.Write(" Top => Location.Y;\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixBuiltInType()));
            this.Write(" Bottom => Location.Y + Size.Height;\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixPointStructName()));
            this.Write(" TopLeft => Location;\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixPointStructName()));
            this.Write(" TopRight => new(Right, Top);\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixPointStructName()));
            this.Write(" BottomLeft => new(Left, Bottom);\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixPointStructName()));
            this.Write(" BottomRight => new(Right, Bottom);\r\n\r\n        public bool IsInside(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixSizeStructName()));
            this.Write(" border) => 0 <= Left && Right <= border.Width && 0 <= Top && Bottom <= border.He" +
                    "ight;\r\n        public bool IsOutside(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixSizeStructName()));
            this.Write(" border) => !IsInside(border);\r\n");
 } 
 if (HasFlag(RoixStructGeneratorOptions.XYPair)) { 
            this.Write("        // RoixStructGeneratorOptions.XYPair\r\n        public bool IsIncludeZero =" +
                    "> (X == 0 || Y == 0);\r\n        public bool IsInside(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixSizeStructName()));
            this.Write(" border) => (0 <= X && X <= border.Width) && (0 <= Y && Y <= border.Height);\r\n   " +
                    "     public bool IsOutside(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixSizeStructName()));
            this.Write(" border) => !IsInside(border);\r\n");
 } 
 if (HasFlag(RoixStructGeneratorOptions.WithBorder)) { 
            this.Write("        // RoixStructGeneratorOptions.WithBorder\r\n        public bool IsInsideBor" +
                    "der => this.Value.IsInside(this.Border);\r\n        public bool IsOutsideBorder =>" +
                    " !this.IsInsideBorder;\r\n\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" ConvertToNewBorder(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixSizeStructName()));
            this.Write(@" newBorder)
        {
            if (this.Border.IsEmpty || this.Border.IsZero) return this;
            if (newBorder.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            if (newBorder.IsZero) throw new ArgumentException(ExceptionMessages.SizeIsZero);

            return new(this.Value * (newBorder / this.Border), newBorder);
        }
  ");
 if (HasFlag(RoixStructGeneratorOptions.TypeInt)) { 
            this.Write("        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetRoixNameWithoutInt()));
            this.Write(@" ConvertToNewBorder(in RoixSize newBorder)
        {
            if (this.Border.IsEmpty || this.Border.IsZero) return this;
            if (newBorder.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            if (newBorder.IsZero) throw new ArgumentException(ExceptionMessages.SizeIsZero);

            return new(this.Value * (newBorder / this.Border), newBorder);
        }
  ");
 } 
            this.Write("\r\n");
 } 
 if (HasFlag(RoixStructGeneratorOptions.ArithmeticOperator1)) { 
            this.Write("        // RoixStructGeneratorOptions.ArithmeticOperator1\r\n        public static " +
                    "");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" operator +(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value1, in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value2)\r\n        {\r\n            return new(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetOperate2Value(ArithmeticOperators.Add, "value1", "value2")));
            this.Write(");\r\n        }\r\n\r\n        public static ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" operator -(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value1, in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value2)\r\n        {\r\n            return new(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetOperate2Value(ArithmeticOperators.Subtract, "value1", "value2")));
            this.Write(");\r\n        }\r\n");
 } 
 if (HasFlag(RoixStructGeneratorOptions.ArithmeticOperator2)) { 
            this.Write("        // RoixStructGeneratorOptions.ArithmeticOperator2\r\n        public static " +
                    "");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" operator *(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value, double scalar)\r\n        {\r\n            return new(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetOperate1Value(ArithmeticOperators.Multiply, "value", "scalar")));
            this.Write(");\r\n        }\r\n\r\n        public static ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" operator /(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value, double scalar)\r\n        {\r\n            if (scalar == 0) throw new DivideB" +
                    "yZeroException();\r\n            return new(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetOperate1Value(ArithmeticOperators.Divide, "value", "scalar")));
            this.Write(");\r\n        }\r\n\r\n        public static ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" operator *(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value, in RoixRatioXY ratio)\r\n        {\r\n            return new(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetOperatorRoixAndRatio(ArithmeticOperators.Multiply, "value", "ratio")));
            this.Write(");\r\n        }\r\n\r\n        public static ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" operator /(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value, in RoixRatioXY ratio)\r\n        {\r\n            if (ratio.IsIncludeZero) th" +
                    "row new DivideByZeroException();\r\n            return new(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetOperatorRoixAndRatio(ArithmeticOperators.Divide, "value", "ratio")));
            this.Write(");\r\n        }\r\n\r\n        public static RoixRatioXY operator /(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value1, in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" value2)\r\n        {\r\n            if (value2.IsIncludeZero) throw new DivideByZero" +
                    "Exception();\r\n            return new(");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetOperate2Value(ArithmeticOperators.Divide, "value1", "value2")));
            this.Write(");\r\n        }\r\n");
 } 
            this.Write("\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class CodeTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
