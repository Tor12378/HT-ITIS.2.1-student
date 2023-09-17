.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
{
    .method public static void ParseCalcArguments(string[] args,
    [out] float64& val1, [out] valuetype Hw2.CalculatorOperation& operation, [out] float64& val2) cil managed
    {
        .locals init (
            [0] bool
        )
    
        ldarg.0
        call bool Hw2.Parser::IsArgLengthSupported(string[])
        ldc.i4.0
        ceq
        stloc.0
        ldloc.0
        brfalse.s CorrectQuantity
    
        ldstr "Invalid number of arguments specified!"
        newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
        throw
    
        CorrectQuantity: ldarg.1
        ldarg.0
        ldc.i4.0
        ldelem.ref
        call float64 Hw2.Parser::ParseStringToDouble(string)
        stind.r8
        ldarg.2
        ldarg.0
        ldc.i4.1
        ldelem.ref
        call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
        stind.i4
        ldarg.3
        ldarg.0
        ldc.i4.2
        ldelem.ref
        call float64 Hw2.Parser::ParseStringToDouble(string)
        stind.r8
        ret
    }
    
    .method private static bool IsArgLengthSupported(string[] args) cil managed
    {
        ldarg.0
        ldlen
        ldc.i4.3
        ceq
        ret
    }
    
    .method private static float64 ParseStringToDouble(string arg) cil managed
    {
        .locals init
        (
            [0]float64 number,
            [1]bool,
            [2]float64
        )
        
        ldarg.0
        ldloca.s 0
        call bool [System.Runtime]System.Double::TryParse(string,float64&)
        stloc.1
        ldloc.1
        brfalse.s IncorrectInput
        
        ldloc.0
        stloc.2
        ldloc.2
        ret
        
        IncorrectInput: ldstr "The entered argument is not a number"
        newobj instance void[System.Runtime]System.ArgumentException::.ctor(string)
        throw
    }
    
    .method private static valuetype Hw2.CalculatorOperation ParseOperation(string arg) cil managed
    {
    .locals init (
        [0] valuetype Hw2.CalculatorOperation,
        [1] valuetype Hw2.CalculatorOperation
    )
   
    ldarg.0
    ldstr "+"
    call bool [System.Runtime]System.String::op_Equality(string, string)
    brtrue.s FoundPlus
    
    ldarg.0
    ldstr "-"
    call bool [System.Runtime]System.String::op_Equality(string, string)
    brtrue.s FoundMinus
    
    ldarg.0
    ldstr "/"
    call bool [System.Runtime]System.String::op_Equality(string, string)
    brtrue.s FoundDivide
    
    ldarg.0
    ldstr "*"
    call bool [System.Runtime]System.String::op_Equality(string, string)
    brtrue.s FoundMultiply
    
    br.s InvalidOperation

    FoundPlus:
    ldc.i4.0
    br.s SaveOperation
    
    FoundMinus:
    ldc.i4.1
    br.s SaveOperation
    
    FoundDivide:
    ldc.i4.3
    br.s SaveOperation
    
    FoundMultiply:
    ldc.i4.2
    br.s SaveOperation

    InvalidOperation:
    ldstr "Operation specified incorrectly"
    newobj instance void [System.Runtime]System.InvalidOperationException::.ctor(string)
    throw

    SaveOperation:
    stloc.0
    ldloc.0
    stloc.1
    ldloc.1
    ret
    }
}


