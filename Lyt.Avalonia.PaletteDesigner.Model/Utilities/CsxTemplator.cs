namespace Lyt.Avalonia.PaletteDesigner.Model.Utilities;

using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

// TODO: Relocate to Lyt.Templator 

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

            throw new Exception("Template generation failed with no exception thrown");
        }
        catch (CompilationErrorException e)
        {
            Debug.WriteLine(e);
            return string.Join(Environment.NewLine, e.Diagnostics.Select(d => $"// {d}"));
        }
        catch  (Exception ex) 
        { 
            Debug.WriteLine(ex);
            return string.Empty;
        }
    }
}
