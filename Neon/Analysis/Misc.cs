using System;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Analysis
{
    abstract class Node
        {
            public abstract TokenType type { get; }
            public abstract IEnumerable<Node> GetChildren();
        }

        abstract class Expression : Node
        {

        }

        sealed class NumberExpr : Expression
        {
            public NumberExpr(Token _number)
            {
                numberToken = _number;
            }
            
            public override TokenType type => TokenType.NumberExpr;
            public Token numberToken { get; }

            public override IEnumerable<Node> GetChildren()
            {
                yield return numberToken;
            }
        }

        sealed class BinaryExpr : Expression
        {
            public BinaryExpr(Expression _left, Token _operator, Expression _right)
            {
                left = _left;
                binaryOperator = _operator;
                right = _right;
            }

            public override TokenType type => TokenType.BinaryExpr;
            public Expression left { get; }
            public Token binaryOperator { get; }
            public Expression right { get; }

            public override IEnumerable<Node> GetChildren()
            {
                yield return left;
                yield return binaryOperator;
                yield return right;
            }
        }

        sealed class ParenthesisExpr : Expression
        {
            public ParenthesisExpr(Token _open, Expression _expr, Token _close)
            {
                openParenthesis = _open;
                expression = _expr;
                closeParenthesis = _close;
            }

            public override TokenType type => TokenType.ParenthesizedExpr;
            public Token openParenthesis { get; }
            public Expression expression { get; }
            public Token closeParenthesis { get; }

            public override IEnumerable<Node> GetChildren()
            {
                yield return openParenthesis;
                yield return expression;
                yield return closeParenthesis;
            }
        }

        sealed class SyntaxTree
        {
            public SyntaxTree(IEnumerable<string> _diagnostics, Expression _root, Token _endOfFile)
            {
                root = _root;
                endOfFile = _endOfFile;
                diagnostics = _diagnostics.ToArray();
            }

            public IReadOnlyList<string> diagnostics;
            public Expression root { get; }
            public Token endOfFile { get; }
        }

        enum TokenType
        {
            Number, WhiteSpace,
            MathAdd, MathSubtract, MathMultiply, MathDivide,
            OpenParenthesis, CloseParenthesis,
            BadToken, EndOfFile,
            NumberExpr, BinaryExpr, ParenthesizedExpr
        }

        class Token : Node
        {
            public override TokenType type { get; }
            public int position { get; }
            public string text { get; }
            public object value { get; }

            public Token(TokenType _type, int _position, string _text, object _value)
            {
                type = _type;
                position = _position;
                text = _text;
                value = _value;
            }

            public override IEnumerable<Node> GetChildren()
            {
                return Enumerable.Empty<Node>();
            }
        }
}