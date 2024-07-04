using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public static class HTMLTagsHelper
    {
        /// <summary>Bold / Fett</summary>
        public static string StartBold() => "<b>";
        /// <summary>Bold / Fett</summary>
        public static string EndBold() => "</b>";

        /// <summary>Italic / Kursiv</summary>
        public static string StartItalic() => "<i>";
        /// <summary>Italic / Kursiv</summary>
        public static string EndItalic() => "</i>";

        /// <summary>underlined / Unterstrichen</summary>
        public static string StartUnderline() => "<u>";
        /// <summary>underlined / Unterstrichen</summary>
        public static string EndUnderline() => "</u>";

        /// <summary>Crossed Out / Durchgestrichen</summary>
        public static string StartCrossOut() => "<s>";
        /// <summary>Crossed Out / Durchgestrichen</summary>
        public static string EndCrossOut() => "</s>";

        /// <summary>Superscript / Hochgestellt</summary>
        public static string StartSuperscript() => "<sup>";
        /// <summary>Superscript / Hochgestellt</summary>
        public static string EndSuperscript() => "</sup>";

        /// <summary>Subscript / Tiefgestellt</summary>
        public static string StartSubscript() => "<sub>";
        /// <summary>Subscript / Tiefgestellt</summary>
        public static string EndSubscript() => "</sub>";

        /// <summary>Color / Farbe.</summary>
        /// <param name="_colorcode">Colorcode. If code is hexadecimal "#" is required! Examples: "red" "#005500" "#FF000088"</param>
        public static string StartColor(string _colorcode) => $"<color={_colorcode}>";
        /// <summary>Color / Farbe.</summary>
        /// <param name="_colorcode">Colorcode. If code is hexadecimal "#" is required! Examples: "red" "#005500" "#FF000088"</param>
        public static string StartColor(Color _colorcode) => $"<color=#{_colorcode.ToHexString()}>";
        /// <summary>Color / Farbe</summary>
        public static string EndColor() => "</color>";
    }
}
