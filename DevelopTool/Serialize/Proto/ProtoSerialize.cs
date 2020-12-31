using ParserCore;
using Proto;
using System.IO;
public class ProtoSerialize
{
    /// <summary>
    /// 反序列化
    /// </summary>
    public static ProtoFile Deserialize(string content)
    {
        ProtoFile obj;
        var pp = new ProtoParser(Parsing.CharParse(content));
        var bl=pp.TryParseProto(out obj);
        if (!bl)
        {
            throw new System.Exception();
        }
        return obj;
    }

    public static string Serialize(ProtoFile p)
    {
        var sw = new StringWriter();
        Serialize(p, sw);
        return sw.ToString();
    }
    static void Serialize(ProtoFile p, StringWriter sw,string tab="")
    {
        //sw.WriteLine(string.Format("{0}package {1};",tab, p.package));
        //sw.WriteLine(string.Format("{0}option optimize_for = {1};", tab, p.Option));
        //foreach(var it in p.Imports)
        //{
        //    sw.WriteLine(string.Format("{0}import \"{1}\";", tab, it));
        //}

        //foreach (var it in p.Enums)
        //{
        //    sw.WriteLine(string.Format("{0}enum {1} {2}", tab, it.name,it.comment==null?"":"\t\t//"+it.comment));
        //    sw.WriteLine(tab+"{");
        //    foreach(var ie in it.Childs)
        //    {
        //        sw.WriteLine(string.Format("\t{0} {1} = {2}; {3}", tab, ie.name,ie.number, ie.comment == null ? "" : "\t\t//" + ie.comment));
        //    }
        //    sw.WriteLine(tab+"};");
        //}

        //foreach (var it in p.Messages)
        //{
        //    sw.WriteLine(string.Format("{0}message {1} {2}", tab, it.name, it.comment == null ? "" : "\t\t//" + it.comment));
        //    sw.WriteLine(tab+"{");
        //    int i = 1;
        //    foreach (var ie in it.Expres)
        //    {
        //        sw.WriteLine(string.Format("\t{0} {1} {2} {3} = {4}; {5}", tab, ie.distinction, ie.type,ie.name,i++, ie.comment == null ? "" : "\t\t//" + ie.comment));
        //    }
        //    sw.WriteLine(tab+"};");
        //}
    }
}