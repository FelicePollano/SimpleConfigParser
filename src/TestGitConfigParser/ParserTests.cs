using GitConfigParser;
using Sprache;

namespace TestGitConfigParser
{
    public class ParserTests
    {
        [Fact]
        public void comment_simple_semicolon()
        {

            var cm = Parser.comment.Parse(";hello") as ConfigItemComment;
            Assert.Equal(";",cm?.Prefix);
            Assert.Equal("hello",cm?.Comment);
        }
        [Fact]
        public void comment_simple_dash()
        {

            var cm = Parser.comment.Parse("#hello") as ConfigItemComment;
            Assert.Equal("#", cm?.Prefix);
            Assert.Equal("hello", cm?.Comment);
        }
        [Fact]
        public void comment_simple_semicolon_preserve_spaces()
        {
            var cm = Parser.comment.Parse(";  hello") as ConfigItemComment;
            Assert.Equal(";", cm?.Prefix);
            Assert.Equal("  hello", cm?.Comment);
        }
        [Fact]
        public void comment_simple_semicolon_ignore_empty_lines()
        {
            var cm = Parser.comment.Parse(@"




                                            ;  hello") as ConfigItemComment;
            Assert.Equal(";", cm.Prefix);
            Assert.Equal("  hello", cm.Comment);
        }
        [Fact]
        public void section_without_comment_without_subsection()
        {
            
            var sect = @"[mysection]";
            var parsed = Parser.section.Parse(sect) as ConfigItemSection;
            Assert.Equal( "mysection", parsed?.Name);
            Assert.Null(parsed?.Comment);
            Assert.Null(parsed?.Subsection);

        }
        [Fact]
        public void section_without_comment_with_spaces()
        {

            var sect = @"    [ mysection  ]";
            var parsed = Parser.section.Parse(sect) as ConfigItemSection;
            Assert.Equal("mysection", parsed?.Name);
            Assert.Null(parsed?.Comment);
            Assert.Null(parsed?.Subsection);

        }
        [Fact]
        public void section_with_comment_with_subsection()
        {
            var sect = @"[mysection ""sub section""] ; hello";
            var parsed = Parser.section.Parse(sect) as ConfigItemSection;
            Assert.Equal("mysection", parsed?.Name);
            Assert.Equal(" hello", parsed?.Comment.Comment);
            Assert.Equal("sub section", parsed?.Subsection.Identifier);
        }

        [Fact]
        public void section_with_comment_with_subsectionand_spaces()
        {
            var sect = @"[   mysection     ""sub section""] ; hello";
            var parsed = Parser.section.Parse(sect) as ConfigItemSection;
            Assert.Equal("mysection", parsed?.Name);
            Assert.Equal(" hello", parsed?.Comment.Comment);
            Assert.Equal("sub section", parsed?.Subsection.Identifier);
        }

        [Fact]
        public void simple_quotedidentifier()
        {
            var qi = @"""quotedidentifier""";
            var parsed = Parser.quotedIdentifier.Parse(qi) as ConfigItemQuotedIdentifier;
            Assert.Equal(qi.Trim('"'), parsed?.Identifier);
        }
        [Fact]
        public void strangechars_quotedidentifier()
        {
            var qi = @"""quot1 - $& ed 09? identifier""";
            var parsed = Parser.quotedIdentifier.Parse(qi) as ConfigItemQuotedIdentifier;
            Assert.Equal(qi.Trim('"'), parsed?.Identifier);
        }
        [Fact]
        public void quotedidentifier_with_unhandled_escape()
        {
            var qi = @"""quotedident\ifier""";
            var parsed = Parser.quotedIdentifier.Parse(qi) as ConfigItemQuotedIdentifier;
            Assert.Equal("quotedidentifier", parsed?.Identifier);
        }
        [Fact]
        public void quotedidentifier_with_backslash()
        {
            var qi = @"""quotedident\\ifier""";
            var parsed = Parser.quotedIdentifier.Parse(qi) as ConfigItemQuotedIdentifier;
            Assert.Equal(@"quotedident\ifier", parsed?.Identifier);
        }
        [Fact]
        public void quotedidentifier_with_quotes()
        {
            var qi = @"""quotedident\""i\""fier""";
            var parsed = Parser.quotedIdentifier.Parse(qi) as ConfigItemQuotedIdentifier;
            Assert.Equal(@"quotedident""i""fier", parsed?.Identifier);
        }
        [Fact]
        public void quotedvalue_with_more_escapes()
        {
            var qi = @"""quotedident\ni\tfie\br""";
            var parsed = Parser.quotedValue.Parse(qi) as ConfigItemQuotedIdentifier;
            Assert.Equal("quotedident\ni\tfie\br", parsed?.Identifier);
        }
        [Fact]
        public void keyname_simple()
        {
            var kn = "pcd-12-abc";
            var parsed = Parser.keyname.Parse(kn) as ConfigItemKeyName;
            Assert.Equal(kn, parsed?.Name);
        }
        [Fact]
        public void keyname_wrong_digit_in_front()
        {
            var kn = "1pcd-12-abc";
            Assert.Throws<ParseException>(()=>Parser.keyname.Parse(kn));
        }
        [Fact]
        public void keyname_wrong_unexpeced_char()
        {
            var kn = "1pcd-1_2-abc";
            Assert.Throws<ParseException>(() => Parser.keyname.Parse(kn));
        }

    }
}