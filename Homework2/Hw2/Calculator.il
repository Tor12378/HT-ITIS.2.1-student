.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator extends [mscorlib]System.Object
{
    .method static public float64 Calculate(float64 value1, valuetype Hw2.CalculatorOperation operation, float64 value2)
        cil managed
    {   
        .locals init (
            [0] float64,
            [1] float64
        )
        ldarg.1
        switch (Plus, Minus, Multiply, Divide)
    
        br.s Other

        Plus: ldarg.0
        ldarg.2
        add
        stloc.0
        br.s Result
    
        Minus:ldarg.0
        ldarg.2
        sub
        stloc.0
        br.s Result
    
        Multiply: ldarg.0
        ldarg.2
        mul
        stloc.0
        br.s Result
    
        Divide: ldarg.0
        ldarg.2
        div
        stloc.0
        br.s Result
    
        Other: newobj instance void [System.Runtime]System.InvalidOperationException::.ctor()
        throw
    
        Result: ldloc.0
        ret
    }   
}
