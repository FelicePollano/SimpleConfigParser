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
        public static readonly Parser<char> escapedescape = Parse.String("\\\\").Return('\\');
        public static readonly Parser<char> escapedcr = Parse.String("\\n").Return('\n');
        public static readonly Parser<char> escapedtab = Parse.String("\\t").Return('\t');
        public static readonly Parser<char> escapedbs = Parse.String("\\b").Return('\b');
        public static readonly Parser<char> escapedunknown = (from sl in Parse.Char('\\') from any in Parse.CharExcept('"') select any); //to follow all know escaped chars

        public static readonly Parser<ConfigItem> quotedIdentifier = (from bq in Parse.Char('"')
                                                                      from q in ( 
                                                                                escapedquote
                                                                                .Or(escapedescape)
                                                                                .Or(escapedunknown)
                                                                                .Or(Parse.CharExcept('"'))
                                                                              ).Many().Text()
                                                                      from eq in Parse.Char('"')
                                                                      select new ConfigItemQuotedIdentifier(q)).Token();
        public static readonly Parser<ConfigItem> quotedValue = (from bq in Parse.Char('"')
                                                                      from q in (
                                                                                escapedquote
                                                                                .Or(escapedescape)
                                                                                .Or(escapedcr)
                                                                                .Or(escapedtab)
                                                                                .Or(escapedbs)
                                                                                .Or(escapedunknown)
                                                                                .Or(Parse.CharExcept('"'))
                                                                              ).Many().Text()
                                                                      from eq in Parse.Char('"')
                                                                      select new ConfigItemQuotedIdentifier(q)).Token();

        public static readonly Parser<ConfigItem> section = (from sq in Parse.Char('[').Token()
                                                        from name in Parse.LetterOrDigit.Many().Text()
                                                        from subsection in quotedIdentifier.Optional()
                                                        from sqc in Parse.Char(']').Token()
                                                        from cmn in comment.Optional().Token()
                                                        select new ConfigItemSection(name,cmn.GetOrDefault() as ConfigItemComment, subsection.GetOrDefault() as ConfigItemQuotedIdentifier)).Token();
        public static readonly Parser<ConfigItem> keyname= ( from first in Parse.Letter.AtLeastOnce().Text()
                                                             from trailing in Parse.Or(Parse.LetterOrDigit,Parse.Char('-')).Many().Text()
                                                             select new ConfigItemKeyName(first+trailing)
                                                             ).Token();
       /*
        public static readonly Parser<ConfigItem> assign = (from kn in keyname
                                                            from eq in Parse.Char('=').Token()
                                                            from rhs in ( from first in Parse.CharExcept('"').AtLeastOnce().Text()
                                                                          from trail in Parse.CharExcept(";#").Many().Text() select first+trail).Optional())
                                                                          .Or((from v in quotedValue as Parser<ConfigItemQuotedIdentifier> select v.Identifier)) 
                                                            ).Token();*/
       
    }
}