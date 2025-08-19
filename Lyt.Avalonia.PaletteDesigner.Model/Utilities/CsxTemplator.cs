namespace Lyt.Avalonia.PaletteDesigner.Model.Utilities;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

public static class CsxTemplator
{
    public static async Task<string> Generate(string template, string variable)
    {
        try
        {
            var options = ScriptOptions.Default;
            var script = 
                CSharpScript
                    .Create<string>($@"var Namespace = ""{variable}"";")
                    .ContinueWith(template, options);
            var state = await script.RunAsync();
            if (state is not null && state.ReturnValue is not null)
            {
                string? generated = state.ReturnValue.ToString();
                if (!string.IsNullOrWhiteSpace(generated))
                {
                    return generated;
                }
            }

            return string.Empty;
        }
        catch  (Exception ex) 
        { 
            Debug.WriteLine(ex);
            return string.Empty;
        }
    }
}
