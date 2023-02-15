using GitConfigParser;
using Sprache;
using System.Globalization;

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
        public void comment_ignore_leading_spaces()
        {

            var cm = Parser.comment.Parse("     ;hello") as ConfigItemComment;
            Assert.Equal(";", cm?.Prefix);
            Assert.Equal("hello", cm?.Comment);
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
            Assert.Equal("sub section", parsed?.Subsection);
        }

        [Fact]
        public void section_with_comment_with_subsectionand_spaces()
        {
            var sect = @"[   mysection     ""sub section""] ; hello";
            var parsed = Parser.section.Parse(sect) as ConfigItemSection;
            Assert.Equal("mysection", parsed?.Name);
            Assert.Equal(" hello", parsed?.Comment.Comment);
            Assert.Equal("sub section", parsed?.Subsection);
        }

        [Fact]
        public void simple_quotedidentifier()
        {
            var qi = @"""quotedidentifier""";
            var parsed = Parser.quotedIdentifier.Parse(qi);
            Assert.Equal(qi.Trim('"'), parsed);
        }
        [Fact]
        public void strangechars_quotedidentifier()
        {
            var qi = @"""quot1 - $& ed 09? identifier""";
            var parsed = Parser.quotedIdentifier.Parse(qi);
            Assert.Equal(qi.Trim('"'), parsed);
        }
        [Fact]
        public void quotedidentifier_with_unhandled_escape()
        {
            var qi = @"""quotedident\ifier""";
            var parsed = Parser.quotedIdentifier.Parse(qi);
            Assert.Equal("quotedidentifier", parsed);
        }
        [Fact]
        public void quotedidentifier_with_backslash()
        {
            var qi = @"""quotedident\\ifier""";
            var parsed = Parser.quotedIdentifier.Parse(qi);
            Assert.Equal(@"quotedident\ifier", parsed);
        }
        [Fact]
        public void quotedidentifier_with_quotes()
        {
            var qi = @"""quotedident\""i\""fier""";
            var parsed = Parser.quotedIdentifier.Parse(qi) ;
            Assert.Equal(@"quotedident""i""fier", parsed);
        }
        [Fact]
        public void quotedvalue_with_more_escapes()
        {
            var qi = @"""quotedident\ni\tfie\br""";
            var parsed = Parser.quotedValue.Parse(qi) ;
            Assert.Equal("quotedident\ni\tfie\br", parsed);
        }
        [Fact]
        public void keyname_simple()
        {
            var kn = "pcd-12-abc";
            var parsed = Parser.keyname.Parse(kn) ;
            Assert.Equal(kn, parsed);
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
        [Fact]
        public void simple_assignment_wo_value()
        {
            var kn = "key";
            var parsed = Parser.assign.Parse(kn) as ConfigItemAssign;
            Assert.Equal("key", parsed?.Lhs);
            Assert.Null(parsed?.Rhs);
        }
        [Fact]
        public void simple_assignment()
        {
            var kn = "key=value";
            var parsed = Parser.assign.Parse(kn) as ConfigItemAssign;
            Assert.Equal("key", parsed?.Lhs);
            Assert.Equal("value", parsed?.Rhs);
        }
        [Fact]
        public void simple_assignment_with_spaces_1()
        {
            var kn = "key =value";
            var parsed = Parser.assign.Parse(kn) as ConfigItemAssign;
            Assert.Equal("key", parsed?.Lhs);
            Assert.Equal("value", parsed?.Rhs);
        }
        [Fact]
        public void simple_assignment_with_spaces_2()
        {
            var kn = "key = value";
            var parsed = Parser.assign.Parse(kn) as ConfigItemAssign;
            Assert.Equal("key", parsed?.Lhs);
            Assert.Equal("value", parsed?.Rhs);
        }
        [Fact]
        public void simple_assignment_with_spaces_3()
        {
            var kn = "key = value      ";
            var parsed = Parser.assign.Parse(kn) as ConfigItemAssign;
            Assert.Equal("key", parsed?.Lhs);
            Assert.Equal("value", parsed?.Rhs);
        }
        [Fact]
        public void simple_assignment_with_spaces_4()
        {
            var kn = "     key = value      ";
            var parsed = Parser.assign.Parse(kn) as ConfigItemAssign;
            Assert.Equal("key", parsed?.Lhs);
            Assert.Equal("value", parsed?.Rhs);
        }
        [Fact]
        public void simple_assignment_with_spaces_and_comment()
        {
            var kn = "     key = value      ;hello";
            var parsed = Parser.assign.Parse(kn) as ConfigItemAssign;
            Assert.Equal("key", parsed?.Lhs);
            Assert.Equal("value", parsed?.Rhs);
            Assert.NotNull(parsed?.Comment);
            Assert.Equal("hello", parsed?.Comment?.Comment);
        }
        [Fact]
        public void simple_assignment_with_spaces_and_comment_2()
        {
            var kn = "     key = value      #hello";
            var parsed = Parser.assign.Parse(kn) as ConfigItemAssign;
            Assert.Equal("key", parsed?.Lhs);
            Assert.Equal("value", parsed?.Rhs);
            Assert.NotNull(parsed?.Comment);
            Assert.Equal("hello", parsed?.Comment?.Comment);
        }
        [Fact]
        public void simple_assignment_with_spaces_and_quoted_value()
        {
            var kn = "     key = \"value#1\"      #hello";
            var parsed = Parser.assign.Parse(kn) as ConfigItemAssign;
            Assert.Equal("key", parsed?.Lhs);
            Assert.Equal("value#1", parsed?.Rhs);
            Assert.NotNull(parsed?.Comment);
            Assert.Equal("hello", parsed?.Comment?.Comment);
        }
        [Fact]
        public void entire_config()
        {
            var kn = @"
                ;example config
                [ Test ]
                xyz ;no value
                [Test ""ab.cd.;xyz""]  ;with comment
                k=12345
            ";
            var parsed = Parser.config.Parse(kn).ToList();
            Assert.Equal(5,parsed.Count);
            Assert.IsAssignableFrom<ConfigItemComment>( parsed[0]);
            Assert.Equal("example config", (parsed[0] as ConfigItemComment)?.Comment);
            Assert.IsAssignableFrom<ConfigItemSection>(parsed[1]);
            Assert.Equal("Test", (parsed[1] as ConfigItemSection)?.Name);
            Assert.IsAssignableFrom<ConfigItemAssign>(parsed[2]);
            Assert.Equal("xyz", (parsed[2] as ConfigItemAssign)?.Lhs);
            Assert.IsAssignableFrom<ConfigItemSection>(parsed[3]);
            Assert.Equal("Test", (parsed[3] as ConfigItemSection)?.Name);
            Assert.Equal("ab.cd.;xyz", (parsed[3] as ConfigItemSection)?.Subsection);
            Assert.Equal("with comment", (parsed[3] as ConfigItemSection)?.Comment.Comment);
            Assert.IsAssignableFrom<ConfigItemAssign>(parsed[4]);
            Assert.Equal("k", (parsed[4] as ConfigItemAssign)?.Lhs);
            Assert.Equal("12345", (parsed[4] as ConfigItemAssign)?.Rhs);

        }
        [Fact]
        public void using_config_class()
        {
            var kn = @"
                auto   ; explicit true, out of any section
                ;example config
                [ Test ]
                xyz ;no value
                [Test ""ab.cd.;xyz""]  ;with comment
                k=12345
            ";
            Configuration c = new Configuration();
            c.Add(kn);
            Assert.Equal(true.ToString(CultureInfo.InvariantCulture), c["auto"]);
            Assert.True(c.ContainsKey("Test"));
        }
    }
}