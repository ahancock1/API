using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for Session
/// </summary>
[DataContract]
public class Session
{
    //CONSTRUCTOR
	public Session(Int32 inSessionID, string inTitle, DateTime inDate, string staff, string location)
	{
        sessionID = inSessionID;
        title = inTitle;
        date = inDate;
	}

    //SETTERS AND GETTERS
    [DataMember]
    public Int32 sessionID { get; set; }

    [DataMember]
    public string title { get; set; }

    [DataMember]
    public DateTime date { get; set; }

    [DataMember]
    public string staff { get; set; }

    [DataMember]
    public string location { get; set; }

}