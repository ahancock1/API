using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;


/// <summary>
/// Summary description for httpHandler
/// </summary>
public class HttpHandler : IHttpHandler
{
    public HttpHandler()
	{
		// TODO: Add constructor logic here
	}

    public void ProcessRequest(HttpContext context)
    {
        HttpRequest request     = context.Request;

        //remove the path prefex to leave just the URI
        string path = request.Path.Replace("/soft338/ahancock/", "");
        switch (path.ToLower())
        {
            case "module":      moduleHandler(context);                 break;      //process module URI
            case "lecture":     lectureHandler(context);                break;      //process lecture URI
            case "practical":   practicalHandler(context);              break;      //process practical URI
            default:            pageNotFound(context);                  break;      //404 page not found response
        }
    }

    public void moduleHandler(HttpContext context)
    {
        string verb = context.Request.HttpMethod.ToLower();
        switch (verb)
        {
            case "get":         getAllModules(context);                 break;      //gets all the modules
            case "put":         updateModule(context);                  break;      //updates a module
            case "post":        postNewModule(context);                 break;      //posts a new module
            case "delete":      deleteModule(context);                  break;      //deletes a module
            default:                                                    break;
        }
    }


    public void lectureHandler(HttpContext context)
    {
        string verb = context.Request.HttpMethod.ToLower();
        switch (verb)
        {
            case "get":         getAllSessions(context, "lecture");     break;
            case "put":         updateSession(context);                 break;
            case "post":        postNewSession(context);                break;
            case "delete":      deleteSession(context);                 break;
            default:                                                    break;
        }
    }

    public void practicalHandler(HttpContext context)
    {
        string verb = context.Request.HttpMethod.ToLower();
        switch (verb)
        {
            case "get":         getAllSessions(context, "practical");   break;
            case "put":         updateSession(context);                 break;
            case "post":        postNewSession(context);                break;
            case "delete":      deleteSession(context);                 break;
            default:                                                    break;
        }
    }

    #region Post methods
    private void postNewModule(HttpContext context)
    {
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Module));
        Module module = (Module)jsonData.ReadObject(context.Request.InputStream);
        Int32 moduleID = DbCon.insertNewModule(module);
    }

    private void postNewSession(HttpContext context)
    {
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Module));
        Session session = (Session)jsonData.ReadObject(context.Request.InputStream);
        Int32 sessionID = DbCon.insertNewSession(session);
    }
    #endregion

    #region Put methods
    private void updateModule(HttpContext context)
    {
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Module));
        Module module = (Module)jsonData.ReadObject(context.Request.InputStream);
        Int32 moduleID = DbCon.updateModule(module);
        HttpResponse response = context.Response;
        //throw new NotImplementedException();
    }

    private void updateSession(HttpContext context)
    {
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Module));
        Session session = (Session)jsonData.ReadObject(context.Request.InputStream);
        Int32 sessionID = DbCon.updateSession(session);
        HttpResponse response = context.Response;
        //throw new NotImplementedException();
    }
    #endregion

    #region Delete methods
    private void deleteModule(HttpContext context)
    {
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Module));
        Module module = (Module)jsonData.ReadObject(context.Request.InputStream);
        Int32 moduleID = DbCon.deleteModule(module);
    }

    private void deleteSession(HttpContext context)
    {
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Session));
        Session session = (Session)jsonData.ReadObject(context.Request.InputStream);
        Int32 sessionID = DbCon.deleteSession(session);
    }
    #endregion

    #region Get all methods
    private void getAllModules(HttpContext context)
    {
        Stream outputStream = context.Response.OutputStream;
        context.Response.ContentType = "application/json";

        //loops through all the module entries and outputs them in json format
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(IEnumerable<Module>));
        IEnumerable<Module> modules = DbCon.getAllmodules();
        jsonData.WriteObject(outputStream, modules);
    }

    private void getAllSessions(HttpContext context, String comparable)
    {
        Stream outputStream = context.Response.OutputStream;
        context.Response.ContentType = "application/json";

        //loops through all the module entries and outputs them in json format
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(IEnumerable<Session>));
        IEnumerable<Session> session = DbCon.getAllSessions(comparable);

        jsonData.WriteObject(outputStream, session);
    }
    #endregion 

    public bool IsReusable
    {
        // To enable pooling, return true here.
        // This keeps the handler in memory.
        get { return true; }
    }

    private void pageNotFound(HttpContext context)
    {
        HttpResponse response = context.Response;

        //default response where no page can be found or doesnt exist
        response.Write("<html>");
        response.Write("<body>");
        response.Write("<h1>404 Error: Page not found<h1>");
        response.Write("<p>This is the default response from the HttpHandler when no page can be found</p>");
        response.Write("<p>Check back later to see if the page has been added</p>");
        response.Write("</body>");
        response.Write("</html>");
    }

}