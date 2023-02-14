using Sprache;

namespace GitConfigParser
{
    /*
     * https://git-scm.com/docs/git-config#_configuration_file
     */
    
    public class Parser
    {
        public static readonly Parser<ConfigItem> comment = (from bc in Parse.Or(Parse.Char(';'), Parse.Char('#')).AtLeastOnce().Text()
                                                        from ctext in Parse.Or(Parse.CharExcept("\r\n"), Parse.LetterOrDigit).Many().Text()
                                                        select new ConfigItemComment(bc,ctext)).Token();
        public static readonly Parser<char> escapedquote = Parse.String("\\\"").Return('"');
        public static readonly Parser<char> escapedescape = Parse.String("\\\\").Return('\\');
        public static readonly Parser<char> escapedcr = Parse.String("\\n").Return('\n');
        public static readonly Parser<char> escapedtab = Parse.String("\\t").Return('\t');
        public static readonly Parser<char> escapedbs = Parse.String("\\b").Return('\b');
        public static readonly Parser<char> escapedunknown = (from sl in Parse.Char('\\') from any in Parse.CharExcept('"') select any); //to follow all know escaped chars

        public static readonly Parser<string> quotedIdentifier = (from bq in Parse.Char('"')
                                                                      from q in ( 
                                                                                escapedquote
                                                                                .Or(escapedescape)
                                                                                .Or(escapedunknown)
                                                                                .Or(Parse.CharExcept('"'))
                                                                              ).Many().Text()
                                                                      from eq in Parse.Char('"')
                                                                      select q).Token();
        public static readonly Parser<string> quotedValue = (from bq in Parse.Char('"')
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
                                                                      select q).Token();

        public static readonly Parser<ConfigItem> section = (from sq in Parse.Char('[').Token()
                                                        from name in Parse.LetterOrDigit.Many().Text()
                                                        from subsection in quotedIdentifier.Optional()
                                                        from sqc in Parse.Char(']').Token()
                                                        from cmn in comment.Optional().Token()
                                                        select new ConfigItemSection(name,cmn.GetOrDefault() as ConfigItemComment, subsection.GetOrDefault())).Token();
        public static readonly Parser<string> keyname= ( from first in Parse.Letter.AtLeastOnce().Text()
                                                             from trailing in Parse.Or(Parse.LetterOrDigit,Parse.Char('-')).Many().Text()
                                                             select first+trailing
                                                             ).Token();
       
        public static readonly Parser<ConfigItem> assign = (from kn in keyname.Token()
                                                            from rhs in (from eq in Parse.Char('=').Token()
                                                            from rhs in (quotedValue.Or(Parse.CharExcept(" ;#").Many().Text()).Token()).Token()
                                                            select rhs
                                                            ).Optional()
                                                            from cmn in comment.Optional().Token()
                                                            select new ConfigItemAssign(kn,rhs.GetOrDefault(),cmn.GetOrDefault() as ConfigItemComment)).Token();

        public static readonly Parser<IEnumerable<ConfigItem>> config = (from c in comment.
                                                                                   Or(section).
                                                                                   Or(assign)
                                                                         select c).Many();


       
    }
}