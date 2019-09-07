using System;
using System.Collections;

public class BalanceOfRequest : ProgramRequest<int>
{
    public BalanceOfRequest(byte[] programAddress) : base(programAddress) { }

    protected override int ParseResult(string elem)
    {
        return ExploadTypeConverters.ParseInt32(elem);
    }

    public IEnumerator BalanceOf(byte[] arg0)
    {
        yield return SendRequest("BalanceOf", new string[] { ExploadTypeConverters.PrintBytes(arg0) });
    }
}
public class EmitRequest : ProgramRequest<object>
{
    public EmitRequest(byte[] programAddress) : base(programAddress) { }

    protected override object ParseResult(string elem)
    {
        return ExploadTypeConverters.ParseNull(elem);
    }

    public IEnumerator Emit(byte[] arg0, int arg1)
    {
        yield return SendRequest("Emit", new string[] { ExploadTypeConverters.PrintBytes(arg0), ExploadTypeConverters.PrintInt32(arg1) });
    }
}
public class HelloWorldRequest : ProgramRequest<string>
{
    public HelloWorldRequest(byte[] programAddress) : base(programAddress) { }

    protected override string ParseResult(string elem)
    {
        return ExploadTypeConverters.ParseUtf8(elem);
    }

    public IEnumerator HelloWorld()
    {
        yield return SendRequest("HelloWorld", new string[] { });
    }
}
public class InsuranceRequest : ProgramRequest<bool>
{
    public InsuranceRequest(byte[] programAddress) : base(programAddress) { }

    protected override bool ParseResult(string elem)
    {
        return ExploadTypeConverters.ParseBool(elem);
    }

    public IEnumerator Insurance(string arg0, int arg1)
    {
        yield return SendRequest("Insurance", new string[] { ExploadTypeConverters.PrintUtf8(arg0), ExploadTypeConverters.PrintInt32(arg1) });
    }
}
public class NewVirusRequest : ProgramRequest<object>
{
    public NewVirusRequest(byte[] programAddress) : base(programAddress) { }

    protected override object ParseResult(string elem)
    {
        return ExploadTypeConverters.ParseNull(elem);
    }

    public IEnumerator NewVirus(string arg0, int arg1)
    {
        yield return SendRequest("NewVirus", new string[] { ExploadTypeConverters.PrintUtf8(arg0), ExploadTypeConverters.PrintInt32(arg1) });
    }
}
public class RentRequest : ProgramRequest<object>
{
    public RentRequest(byte[] programAddress) : base(programAddress) { }

    protected override object ParseResult(string elem)
    {
        return ExploadTypeConverters.ParseNull(elem);
    }

    public IEnumerator Rent(string arg0, int arg1)
    {
        yield return SendRequest("Rent", new string[] { ExploadTypeConverters.PrintUtf8(arg0), ExploadTypeConverters.PrintInt32(arg1) });
    }
}
public class WinRequest : ProgramRequest<object>
{
    public WinRequest(byte[] programAddress) : base(programAddress) { }

    protected override object ParseResult(string elem)
    {
        return ExploadTypeConverters.ParseNull(elem);
    }

    public IEnumerator Win(byte[] arg0, string arg1)
    {
        yield return SendRequest("Win", new string[] { ExploadTypeConverters.PrintBytes(arg0), ExploadTypeConverters.PrintUtf8(arg1) });
    }
}
public class getInsuranceRequest : ProgramRequest<object>
{
    public getInsuranceRequest(byte[] programAddress) : base(programAddress) { }

    protected override object ParseResult(string elem)
    {
        return ExploadTypeConverters.ParseNull(elem);
    }

    public IEnumerator getInsurance(string arg0, string arg1, int arg2, byte[] arg3, int arg4)
    {
        yield return SendRequest("getInsurance", new string[] { ExploadTypeConverters.PrintUtf8(arg0), ExploadTypeConverters.PrintUtf8(arg1), ExploadTypeConverters.PrintInt32(arg2), ExploadTypeConverters.PrintBytes(arg3), ExploadTypeConverters.PrintInt32(arg4) });
    }
}
public class isLostRequest : ProgramRequest<bool>
{
    public isLostRequest(byte[] programAddress) : base(programAddress) { }

    protected override bool ParseResult(string elem)
    {
        return ExploadTypeConverters.ParseBool(elem);
    }

    public IEnumerator isLost(string arg0, string arg1)
    {
        yield return SendRequest("isLost", new string[] { ExploadTypeConverters.PrintUtf8(arg0), ExploadTypeConverters.PrintUtf8(arg1) });
    }
}