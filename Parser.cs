using Sprache;

namespace GitConfigParser
{
    /*
     * https://git-scm.com/docs/git-config#_configuration_file
     */
    
    public class Parser
    {
        public static readonly Parser<ConfigItem> comment = (from bc in Parse.Or(Parse.Char(';'), Parse.Char('#')).AtLeastOnce().Text()
                                                        from ctext in Parse.Or(Parse.WhiteSpace, Parse.LetterOrDigit).Many().Text()
                                                        select new ConfigItemComment(bc,ctext)).Token();
        public static readonly Parser<char> escapedquote = Parse.String("\\\"").Return('"');
        public static readonly Parser<char> escapdescape = Parse.String("\\\\").Return('\\');

        public static readonly Parser<ConfigItem> quotedIdentifier = (  from bq in Parse.Char('"')
                                                                        from q in (
                                                                            Parse.Or(
                                                                                    Parse.Or(Parse.String("\\\"").Return('"'), Parse.String("\\\\").Return('\\'))
                                                                                    ,Parse.Or( (from sl in Parse.Char('\\') from any in Parse.CharExcept('"') select any), Parse.CharExcept('"'))
                                                                                )).Many().Text()
                                                                        from eq in Parse.Char('"') 
                                                                        select new ConfigItemQuotedIdentifier(q)).Token();
                                        
        public static readonly Parser<ConfigItem> section = (from sq in Parse.Char('[')
                                                        from name in Parse.LetterOrDigit.Many().Text()
                                                        from subsection in quotedIdentifier.Optional()
                                                        from sqc in Parse.Char(']')
                                                        from cmn in comment.Optional().Token()
                                                        select new ConfigItemSection(name,cmn.GetOrDefault() as ConfigItemComment, subsection.GetOrDefault() as ConfigItemQuotedIdentifier)).Token();
        public static readonly Parser<ConfigItem> keyname= ( from first in Parse.Letter.AtLeastOnce().Text()
                                                             from trailing in Parse.Or(Parse.LetterOrDigit,Parse.Char('-')).Many().Text()
                                                             select new ConfigItemKeyName(first+trailing)
                                                             ).Token();
        
    }
}