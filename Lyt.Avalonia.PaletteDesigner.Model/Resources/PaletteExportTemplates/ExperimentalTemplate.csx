#r "System"

using System;
using System.Collections.Generic;
using System.Linq;

var powers = Enumerable.Range(10, 5);

$@"// Generation time: {DateTime.Now}

namespace {Namespace}
{{
    public static class QuickMath
    {{
        {string.Join
            (
                Environment.NewLine,
                powers.Select(i => $@"public static double TwoToThePowerOf{i} => {Math.Pow(2, i)};")
            )}
    }}
}}
"