using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for Module
/// </summary>
[DataContract]
public class Module
{
    //CONSTRUCTOR
    //[DataMember]
    public Module(Int32 inModuleID, string inTitle, string inCode, string inTerm) 
    {
        moduleID = inModuleID;
        title = inTitle;
        code = inCode;
        term = inTerm;
	}

    //SETTERS AND GETTERS
    [DataMember]
    public Int32 moduleID { get; set; }

    [DataMember]
    public string title { get; set; }

    [DataMember]
    public string code { get; set; }

    [DataMember]
    public string term { get; set; }

    [DataMember]
    public Session[] lectures { get; set; }

    [DataMember]
    public Session[] practicals { get; set; }
}