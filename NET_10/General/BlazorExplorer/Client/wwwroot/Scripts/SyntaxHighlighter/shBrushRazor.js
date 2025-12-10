/**
 * SyntaxHighlighter
 * http://alexgorbatchev.com/SyntaxHighlighter
 *
 * SyntaxHighlighter is donationware. If you are using it, please donate.
 * http://alexgorbatchev.com/SyntaxHighlighter/donate.html
 *
 * @version
 * 3.0.83 (July 02 2010)
 * 
 * @copyright
 * Copyright (C) 2004-2010 Alex Gorbatchev.
 *
 * @license
 * Dual licensed under the MIT and GPL licenses.
 */
; (function () {
    // CommonJS
    typeof (require) != 'undefined' ? SyntaxHighlighter = require('shCore').SyntaxHighlighter : null;

    function Brush() {
        var keywords = 'abstract as base bool break byte case catch char checked class const ' +
            'continue decimal default delegate do double else enum event explicit ' +
            'extern false finally fixed float for foreach get goto if implicit in int ' +
            'interface internal is lock long namespace new null object operator out ' +
            'override params private protected public readonly ref return sbyte sealed set ' +
            'short sizeof stackalloc static string struct switch this throw true try ' +
            'typeof uint ulong unchecked unsafe ushort using virtual void while ' +
            'async await var where'
            ,
            razorDirectives = 'attribute code functions implements inherits inject layout model namespace page section ' + //not 'using' - keyword
            'attributes bind on ref typeparam'
            ;

        function fixComments(match, regexInfo) {
            var css = (match[0].indexOf("///") == 0)
                ? 'color1'
                : 'comments'
                ;

            return [new SyntaxHighlighter.Match(match[0], match.index, css)];
        }

        this.forHtmlScript(SyntaxHighlighter.regexLib.aspScriptTags);

        function processTags(match, regexInfo) {
            var constructor = SyntaxHighlighter.Match,
                code = match[0],
                tag = new XRegExp('(&lt;|<)[\\s\\/\\?]*(?<name>[:\\w-\\.]+)', 'xg').exec(code),
                result = []
                ;
          
            if (match.attributes != null) {
                var attributes,
                    regex = new XRegExp('(?<name> [\\w:\\-\\.]+)' +
                        '\\s*=\\s*' +
                        //'(?<value> ".*?"|\'.*?\'|\\w+)',
                        '(?<value> (["\'])([^"\']*("[^"\'=]*")*[^"\']*)*\\3|\\w+)',
                        'xg');
                
                while ((attributes = regex.exec(code)) != null) {
                    result.push(new constructor(attributes.name, match.index + attributes.index, 'attribute'));
                    //result.push(new constructor(attributes.value, match.index + attributes.index + attributes[0].indexOf(attributes.value), 'string'));
                    procesStringRazor({ 0: attributes.value, index: match.index + attributes.index + attributes[0].indexOf(attributes.value) }, null, result);
                }
            }
            
            if (tag != null)
                result.push(
                    new constructor(tag.name, match.index + tag[0].indexOf(tag.name), 'tag')
                );

            return result;
        }

        function procesStringRazor(match, regexInfo, result) {
            result = result || [];

            var constructor = SyntaxHighlighter.Match,
                code = match[0],
                //regex = new XRegExp('(?<razor> @?)[^"\']*(?<string> "[^@"]+"|")', 'xg'),
                regex = /(@?)[^"']*((["'])[^"'@]*\3|["']$|^["'])/gm,
                parts
                ;
            
            while ((parts = regex.exec(code)) != null) {
                if (parts[1])
                    result.push(new constructor(parts[1], match.index + parts.index, 'razor'));
                result.push(new constructor(parts[2], match.index + parts.index + parts[0].indexOf(parts[2]), 'string'));
            }
            
            return result;
        }

        function processCSharpTypes(match, regexInfo, result) {
            result = result || [];

            if (match.types) {
                var constructor = SyntaxHighlighter.Match,
                    regex = /[^\w_]([A-Z][\w_]*)\s*([^\w_])/g,
                    parts
                    ;

                var modifierIs = function (modi) {
                    return (match.modifiers != null && match.modifiers.indexOf(modi) != -1);
                }

                while ((parts = regex.exec(":" + match.types)) != null) {
                    var endWith = parts[2];
                    if ((modifierIs("new") || endWith != "(") &&
                        (modifierIs("class") || modifierIs("new") || endWith != "{") &&
                        (!modifierIs("return") || modifierIs("new") || endWith == ".") &&
                        ((!modifierIs(",") && !modifierIs("{") && !modifierIs("(")) || (",)}".indexOf(endWith) == -1)))
                        result.push(new constructor(parts[1], match.index + match[0].indexOf(parts[1]), 'type'));
                }
                regexInfo.regex.lastIndex--;
            }
            return result;
        }

        function DetectCSharpTypes(match, regexInfo, result) {
            result = result || [];

            if (match.types) {
                var regex = /(^|\W)([A-Z]\w+)((\W|\&lt;|\&gt;)+)(?=[\w\r\n]|$)/g,
                    parts
                    ;
                
                var isContains = function (str, keys) {
                    if (!keys) return (!str || str == "");
                    var rx = keys instanceof RegExp ? keys : new RegExp(keys.replace(/([^\w|])/gm, "\\$1"));
                    return (str && rx.test(str));
                }

                var modifierIs = function (keys) {
                    return isContains(match.modifiers, keys);
                }

                while ((parts = regex.exec(match.types)) != null) {
                    var startWith = parts[1].trim()[0],
                        endWith = parts[3].trim()[0];
                    if ((modifierIs("class|new") || !endWith || isContains(".<>&", endWith))
                        || (startWith == "["))
                        result.push(parts[2]);
                    regex.lastIndex--;
                }
                regexInfo.regex.lastIndex--;
            }
            return result;
        }

        function processCSharp(match, regexInfo, result) {
            result = result || [];

            var regex0 = rxCSharpTypesBlockRx,
                code = match[0],
                match1,
                a = [];

            while ((match1 = regex0.exec(code)) != null) {
                match1.index += match.index;
                DetectCSharpTypes(match1, { regex: regex0 }, a);
            }

            if (a.length > 0) {
                regex0 = new RegExp("(^|[^\\w_])(" + a.join("|") + ")($|[^\\w_])", "gm");
                var constructor = SyntaxHighlighter.Match;
                while ((match1 = regex0.exec(code)) != null) {
                    result.push(new constructor(match1[2], match.index + match1.index + (match1[1] == null ? 0 : 1), 'type'));
                }
            }

            return result;
        }

        var keywords4Rx = this.getKeywords(keywords),
            razorDirectives4Rx = this.getKeywords(razorDirectives),
            rxCSharpTypesBlockRx = new XRegExp(
                "(^\\s*|@|(?<modifiers>((public|private|protected|override|class|async|static|new|event|return|as)\\s+|(\\,|\\{|\\(|=)\\s*)+))(?!"
                + keywords4Rx
                + "|[_\\w]+\\s*=)(?<types>[A-Z\\[]([\\w_\\:\\,\\s\\<\\>\\[\\]]|\\&lt;|\\&gt;)+[^\\s])", 'gm'
            );
        this.regexList = [
            { regex: new XRegExp('(\\&lt;|<)\\!\\[[\\w\\s]*?\\[(.|\\s)*?\\]\\](\\&gt;|>)', 'gm'), css: 'color2' },	// <![ ... [ ... ]]>
            { regex: SyntaxHighlighter.regexLib.xmlComments, css: 'comments' },	// <!-- ... -->            
            { regex: SyntaxHighlighter.regexLib.singleLineCComments, func: fixComments },		// one line comments
            { regex: SyntaxHighlighter.regexLib.multiLineCComments, css: 'comments' },			// multiline comments
            { regex: /@\*[\s\S]*\*\@/g, css: 'comments' },			// multiline comments
            { regex: /@"(?:[^"]|"")*"/g, css: 'string' },			// @-quoted strings          

            { regex: /@\w*\s*\{[\s\S]+(?=\})/gm, func: processCSharp },
            { regex: /@\([^@\n\r]+(?=\))|@\w+[^@\n\r]+/gm, func: processCSharp },
            { regex: new RegExp("@(" + razorDirectives4Rx + ")(?=[^\\w-])|@", 'g'), css: 'razor' },			// Razor directive
            { regex: new RegExp(keywords4Rx, 'gm'), css: 'keyword' },			// c# keyword
            { regex: new XRegExp('(&lt;|<)[\\/\\?]*(\\w+)(?<attributes>.*?)[\\s\\/\\?]*(&gt;|>)', 'sg'), func: processTags },
            
            //{ regex: /@.*?(?=\s|\.|\(|{|[^0-9a-zA-Z_-])/g, css: 'razor' },			// Razor directive            
            { regex: SyntaxHighlighter.regexLib.doubleQuotedString, func: procesStringRazor },			// strings
            { regex: SyntaxHighlighter.regexLib.singleQuotedString, func: procesStringRazor },			// strings
            //{ regex: SyntaxHighlighter.regexLib.doubleQuotedString, css: 'string' },			// strings
            //{ regex: SyntaxHighlighter.regexLib.singleQuotedString, css: 'string' },			// strings
            
            { regex: /^\s*#.*/gm, css: 'preprocessor' },		// preprocessor tags like #region and #endregion
            { regex: /\bpartial(?=\s+(?:class|interface|struct)\b)/g, css: 'keyword' },			// contextual keyword: 'partial'
            { regex: /\byield(?=\s+(?:return|break)\b)/g, css: 'keyword' },			// contextual keyword: 'yield'
        ];
    };

    Brush.prototype = new SyntaxHighlighter.Highlighter();
    Brush.aliases = ['rzr', 'razorc#', 'blazor'];

    SyntaxHighlighter.brushes.Razor = Brush;

    // CommonJS
    typeof (exports) != 'undefined' ? exports.Brush = Brush : null;
})();
