using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Strings;

namespace MyProject.Extensions
{
	public static class PaceConverterExtension
    {
		public static IHtmlEncodedString ConvertPace(this IHtmlEncodedString input)
		{
		
			const string pattern = @"\{\{(\d+):(\d+)(,mi)?\}\}";
			string? formattedString = input.ToString();

			if (string.IsNullOrEmpty(formattedString))
			{
				return new HtmlEncodedString(string.Empty);
			}

			var matches = Regex.Matches(formattedString, pattern).Cast<Match>().ToList();
			var sb = new StringBuilder(formattedString);

			foreach (var match in matches)
			{
				int minutes = int.Parse(match.Groups[1].Value);
				int seconds = int.Parse(match.Groups[2].Value);
				bool isMiles = match.Groups[3].Success; // Check if ",mi" is present

				double totalSeconds = minutes * 60 + seconds;
				double convertedPaceInMinutes;
				string originalPace, convertedPace;

				if (isMiles)
				{
					// Convert pace from min/mi to min/km
					convertedPaceInMinutes = totalSeconds / 1.609344 / 60;
					originalPace = $"{minutes}:{seconds:00}min/mi";
					convertedPace = $"{TimeSpan.FromMinutes(convertedPaceInMinutes):m\\:ss}min/km";
				}
				else
				{
					// Convert pace from min/km to min/mi
					convertedPaceInMinutes = totalSeconds * 1.609344 / 60;
					originalPace = $"{minutes}:{seconds:00}min/km";
					convertedPace = $"{TimeSpan.FromMinutes(convertedPaceInMinutes):m\\:ss}min/mi";
				}

				// Replace the original token with the converted pace using StringBuilder
				sb.Replace(match.Value, $"{originalPace} ({convertedPace})");
			}

			return new HtmlEncodedString(sb.ToString());
		}



	}
}
            
      