using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace HoangViet.Helpers;

public static class LessonContentFormatter
{
    public static string RenderTheoryBentoHtml(string? rawTheory)
    {
        if (string.IsNullOrWhiteSpace(rawTheory))
            return string.Empty;

        var normalized = rawTheory.Replace("\r\n", "\n").Replace("\r", "\n");
        var rawLines = normalized.Split('\n');

        // Trim common leading indentation from raw string literals
        int minIndent = int.MaxValue;
        foreach (var line in rawLines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            int spaces = 0;
            while (spaces < line.Length && line[spaces] == ' ') spaces++;
            if (spaces < minIndent) minIndent = spaces;
        }
        if (minIndent == int.MaxValue) minIndent = 0;

        var lines = rawLines
            .Select(l => l.Length >= minIndent ? l.Substring(minIndent) : l.TrimStart())
            .ToList();

        // Group lines into logical Bento sections
        var sections = new List<List<string>>();
        List<string>? currentSection = null;
        bool inCodeFence = false;

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if (trimmed.StartsWith("```"))
            {
                inCodeFence = !inCodeFence;
                currentSection ??= new List<string>();
                currentSection.Add(line);
                continue;
            }

            if (!inCodeFence && IsTopLevelSectionStart(line, trimmed))
            {
                currentSection = new List<string> { trimmed };
                sections.Add(currentSection);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(trimmed) && currentSection == null)
                    continue;

                if (currentSection == null)
                {
                    currentSection = new List<string>();
                    sections.Add(currentSection);
                }
                currentSection.Add(line);
            }
        }

        var sb = new StringBuilder();
        sb.Append("<div class=\"hv-bento-grid\">");

        foreach (var sec in sections)
        {
            if (sec.Count == 0) continue;
            sb.Append(RenderSingleBentoCard(sec));
        }

        sb.Append("</div>");
        return sb.ToString();
    }

    private static bool IsTopLevelSectionStart(string line, string trimmed)
    {
        if (string.IsNullOrWhiteSpace(trimmed)) return false;

        int leadingSpaces = 0;
        while (leadingSpaces < line.Length && line[leadingSpaces] == ' ') leadingSpaces++;

        if (trimmed.StartsWith("• "))
            return true;

        // Top-level "+ 1." or "+ **1." or "+ **💡"
        if (leadingSpaces <= 2 && (trimmed.StartsWith("+ **") || Regex.IsMatch(trimmed, @"^\+\s*\d+\.")))
            return true;

        return false;
    }

    private static string RenderSingleBentoCard(List<string> sectionLines)
    {
        string firstLine = sectionLines[0].Trim();
        if (firstLine.StartsWith("• "))
            firstLine = firstLine.Substring(2).Trim();
        else if (firstLine.StartsWith("+ "))
            firstLine = firstLine.Substring(2).Trim();

        string cardClass = ClassifyCardType(firstLine, sectionLines, out string badgeIcon, out string badgeLabel);

        var sb = new StringBuilder();
        sb.Append($"<div class=\"hv-bento-card {cardClass}\">");

        // Header of the Bento card
        sb.Append("<div class=\"hv-bento-card-header\">");
        sb.Append($"<span class=\"hv-bento-badge\">{badgeIcon} {badgeLabel}</span>");

        // Check if firstLine is "Title: content" where content is long and there are no body lines
        string headerTitle = firstLine;
        string? inlineFirstBody = null;
        string cleanFirst = StripMarkdownBold(firstLine);

        int colonIdx = firstLine.IndexOf(':');
        if (colonIdx > 0 && colonIdx < firstLine.Length - 1)
        {
            string afterColon = firstLine.Substring(colonIdx + 1).Trim();
            if (afterColon.Length > 25 && sectionLines.Count == 1)
            {
                headerTitle = firstLine.Substring(0, colonIdx + 1);
                inlineFirstBody = afterColon;
            }
        }

        sb.Append($"<div class=\"hv-bento-title\">{FormatInlineText(headerTitle)}</div>");
        sb.Append("</div>");

        // Body of the Bento card
        sb.Append("<div class=\"hv-bento-card-body\">");
        if (!string.IsNullOrWhiteSpace(inlineFirstBody))
        {
            sb.Append($"<div class=\"hv-theory-item\">{FormatInlineText(inlineFirstBody)}</div>");
        }

        RenderBodyLines(sectionLines.Skip(1).ToList(), sb);

        sb.Append("</div>");
        sb.Append("</div>");
        return sb.ToString();
    }

    private static string ClassifyCardType(string titleLine, List<string> allLines, out string icon, out string label)
    {
        string joined = string.Join("\n", allLines);

        if (titleLine.Contains("💡") ||
            ContainsAnyIgnoreCase(titleLine, "Bản chất", "Hiểu đơn giản", "Hiểu bản chất", "Tại sao", "Tổng kết", "Bản đồ"))
        {
            icon = "💡";
            label = "BẢN CHẤT & TƯ DUY CỐT LÕI";
            return "bento-card-analogy";
        }

        if (titleLine.Contains("⚠️") ||
            ContainsAnyIgnoreCase(titleLine, "Bẫy", "Lưu ý", "Quy tắc", "Sai lầm", "Kinh nghiệm", "Điểm cốt lõi"))
        {
            icon = "⚠️";
            label = "LƯU Ý THỰC CHIẾN & BẪY HAY GẶP";
            return "bento-card-warning";
        }

        if (titleLine.Contains("🖼️") ||
            joined.Contains("```") ||
            ContainsAnyIgnoreCase(titleLine, "Sơ đồ", "Cơ chế Bộ nhớ", "Ba loại Delegate", "Hai cách viết"))
        {
            icon = "🖼️";
            label = "SƠ ĐỒ CẤU TRÚC & BỘ NHỚ TRỰC QUAN";
            return "bento-card-diagram";
        }

        if (titleLine.Contains("🔍") ||
            ContainsAnyIgnoreCase(titleLine, "Giải phẫu", "Cú pháp", "Cách đọc", "Cấu trúc", "Bộ đôi", "Các công cụ"))
        {
            icon = "🔍";
            label = "GIẢI PHẪU CÚ PHÁP & CƠ CHẾ";
            return "bento-card-anatomy";
        }

        if (titleLine.Contains("📊") ||
            allLines.Any(l => l.Trim().StartsWith("|") && l.Trim().EndsWith("|")) ||
            ContainsAnyIgnoreCase(titleLine, "Bảng", "So sánh", "Phân biệt", "Ma Trận"))
        {
            icon = "📊";
            label = "PHÂN TÍCH CHUYÊN SÂU & BẢNG ĐỐI CHIẾU";
            return "bento-card-deepdive";
        }

        icon = "📘";
        label = "KIẾN THỨC TRỌNG TÂM";
        return "bento-card-deepdive";
    }

    private static void RenderBodyLines(List<string> lines, StringBuilder sb)
    {
        int i = 0;
        while (i < lines.Count)
        {
            string line = lines[i];
            string trimmed = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                i++;
                continue;
            }

            // 1. Fenced Code / Visual Memory Diagram block
            if (trimmed.StartsWith("```"))
            {
                var diagramSb = new StringBuilder();
                i++;
                while (i < lines.Count && !lines[i].Trim().StartsWith("```"))
                {
                    diagramSb.AppendLine(WebUtility.HtmlEncode(lines[i]));
                    i++;
                }
                if (i < lines.Count && lines[i].Trim().StartsWith("```"))
                    i++;

                sb.Append("<div class=\"hv-memory-diagram-wrap\">");
                sb.Append("<div class=\"hv-memory-diagram-header\"><i class=\"bi bi-diagram-3-fill me-2\"></i>Sơ đồ cấu trúc bộ nhớ & luồng hoạt động</div>");
                sb.Append($"<pre class=\"memory-diagram\"><code>{diagramSb.ToString().TrimEnd()}</code></pre>");
                sb.Append("</div>");
                continue;
            }

            // 2. Markdown Table block
            if (trimmed.StartsWith("|") && trimmed.EndsWith("|"))
            {
                var tableLines = new List<string>();
                while (i < lines.Count && lines[i].Trim().StartsWith("|") && lines[i].Trim().EndsWith("|"))
                {
                    tableLines.Add(lines[i].Trim());
                    i++;
                }
                sb.Append(RenderMarkdownTable(tableLines));
                continue;
            }

            // 3. Regular bullet or numbered sub-item
            int indent = 0;
            while (indent < line.Length && line[indent] == ' ') indent++;

            string itemClass = indent >= 4 ? "hv-theory-subitem" : "hv-theory-item";
            string displayLine = trimmed;

            if (displayLine.StartsWith("- "))
                displayLine = "<span class=\"hv-bullet-dot\">▸</span> " + FormatInlineText(displayLine.Substring(2).Trim());
            else if (displayLine.StartsWith("+ "))
                displayLine = "<span class=\"hv-bullet-plus\">✦</span> " + FormatInlineText(displayLine.Substring(2).Trim());
            else if (Regex.IsMatch(displayLine, @"^\d+\.\s+"))
            {
                var match = Regex.Match(displayLine, @"^(\d+\.)\s+(.*)$");
                displayLine = $"<span class=\"hv-step-num-pill\">{match.Groups[1].Value}</span> " + FormatInlineText(match.Groups[2].Value);
            }
            else
            {
                displayLine = FormatInlineText(displayLine);
            }

            sb.Append($"<div class=\"{itemClass}\">{displayLine}</div>");
            i++;
        }
    }

    private static string RenderMarkdownTable(List<string> tableLines)
    {
        if (tableLines.Count == 0) return string.Empty;

        var sb = new StringBuilder();
        sb.Append("<div class=\"table-responsive hv-table-wrapper\">");
        sb.Append("<table class=\"hv-bento-table\">");

        bool headerRendered = false;
        foreach (var rawRow in tableLines)
        {
            var cells = rawRow.Trim('|')
                .Split('|')
                .Select(c => c.Trim())
                .ToList();

            // Skip separator row like | :--- | :--- |
            if (cells.All(c => Regex.IsMatch(c, @"^:?-{2,}:?$")))
                continue;

            if (!headerRendered)
            {
                sb.Append("<thead><tr>");
                foreach (var cell in cells)
                {
                    sb.Append($"<th>{FormatInlineText(cell)}</th>");
                }
                sb.Append("</tr></thead><tbody>");
                headerRendered = true;
            }
            else
            {
                sb.Append("<tr>");
                foreach (var cell in cells)
                {
                    sb.Append($"<td>{FormatInlineText(cell)}</td>");
                }
                sb.Append("</tr>");
            }
        }

        if (headerRendered)
            sb.Append("</tbody>");

        sb.Append("</table></div>");
        return sb.ToString();
    }

    public static string FormatInlineText(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        // Extract inline code `...` into placeholders first so HTML encoding doesn't break inside or outside
        var codeSegments = new List<string>();
        string processed = Regex.Replace(text, @"`([^`]+)`", m =>
        {
            int idx = codeSegments.Count;
            codeSegments.Add(WebUtility.HtmlEncode(m.Groups[1].Value));
            return $"__CODE_PLACEHOLDER_{idx}__";
        });

        processed = WebUtility.HtmlEncode(processed);

        // Bold **...**
        processed = Regex.Replace(processed, @"\*\*(.+?)\*\*", "<strong class=\"hv-text-strong\">$1</strong>");

        // Italic *...*
        processed = Regex.Replace(processed, @"\*([^*]+)\*", "<em class=\"hv-text-em\">$1</em>");

        // Arrows -> or ➔ or $\rightarrow$
        processed = processed.Replace("$\\rightarrow$", "<span class=\"hv-inline-arrow\">➔</span>");
        processed = processed.Replace("-&gt;", "<span class=\"hv-inline-arrow\">➔</span>");

        // Restore inline code placeholders
        for (int i = 0; i < codeSegments.Count; i++)
        {
            processed = processed.Replace(
                $"__CODE_PLACEHOLDER_{i}__",
                $"<code class=\"hv-inline-code\">{codeSegments[i]}</code>");
        }

        return processed;
    }

    private static string StripMarkdownBold(string input)
    {
        return input.Replace("**", "");
    }

    private static bool ContainsAnyIgnoreCase(string text, params string[] keywords)
    {
        foreach (var kw in keywords)
        {
            if (text.Contains(kw, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}
