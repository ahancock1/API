using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for dbCon
/// </summary>
public class DbCon
{
    private static string connectionString = WebConfigurationManager.ConnectionStrings["SOFT338_ConnectionString"].ConnectionString;
    public static string lastError = "";

	public DbCon()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region Get methods
    public static List<Module> getAllmodules()
    {
        List<Module> modules = new List<Module>();
        
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Module", con);
        using (con) {
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               Module module = new Module((Int32)reader["ModuleID"], (string)reader["Title"], (string)reader["Code"], (string)reader["Term"]);
               modules.Add(module);
            }
        }
        
        return modules;
    }

    public static List<Session> getAllSessions(string comparable)
    {
        List<Session> sessions = new List<Session>();

        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Session", con);
        using (con)
        {
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Session session = new Session((Int32)reader["SessionID"], (string)reader["Description"], (DateTime)reader["Date"], (string)reader["Staff_Member"], (string)reader["Location"]);
                if (session.title.Contains(comparable))
                {
                    sessions.Add(session);
                }
            }
        }

        return sessions;
    }

    public static Module getModuleByID(Int32 ID)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Module WHERE ModuleID='" + ID.ToString() + "'", con);

        using (con)
        {
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Module module = new Module((Int32)reader["ModuleID"], (string)reader["Title"], (string)reader["Code"], (string)reader["Term"]);
                return module;
            }
        }

        return null;
    }

    public static Session getSessionByID(Int32 ID)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Session WHERE SessionID='" + ID.ToString() + "'", con);
        
        using (con)
        {
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Session session = new Session((Int32)reader["SessionID"], (string)reader["Description"], (DateTime)reader["Date"], (string)reader["Staff_Member"], (string)reader["Location"]);
                return session;
            }
        }

        return null;
    }

    #endregion

    #region Insert methods
    public static Int32 insertNewModule(Module module)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("INSERT INTO Module (Title, Code, Term) VALUES('"
            + module.title + "', '"
            + module.code + "', '"
            + module.term + "'); " + " SELECT CAST(Scope_Indentity() as int)", con);

        Int32 returnID = 0;
        using (con)
        {
            try
            {
                con.Open();
                returnID = (Int32)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }

        return returnID;
    }

    public static Int32 insertNewSession(Session session)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("INSERT INTO session (Date, Description, Location, Staff_Member) VALUES('"
            + session.date + "'. '"
            + session.title + "', '"
            + session.location + "', '"
            + session.staff + "'); " + " SELECT CAST(Scope_Indentity() as int)", con);

        Int32 returnID = 0;
        using (con)
        {
            try
            {
                con.Open();
                returnID = (Int32)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }

        return returnID;
    }
    #endregion

    #region Delete methods
    public static Int32 deleteModule(Module module)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("DELETE FROM Module WHERE ModuleID='" 
            + module.moduleID + "'", con);

        Int32 returnID = 0;
        using (con)
        {
            try
            {
                con.Open();
                returnID = (Int32)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }

        return returnID;
    }

    public static Int32 deleteSession(Session session)
    {
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("DELETE FROM Session WHERE SessionID='"
            + session.sessionID + "'", con);

        Int32 returnID = 0;
        using (con)
        {
            try
            {
                con.Open();
                returnID = (Int32)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }

        return returnID;
    }
    #endregion

    #region Update methods
    public static Int32 updateModule(Module newModule)
    {
        Module module = getModuleByID(newModule.moduleID);

        //check for unfilled details
        if (newModule.title == null) newModule.title = module.title;
        if (newModule.term == null) newModule.term = module.term;
        //if (newModule.lectures == null) newModule.lectures = module.lectures;
        //if (newModule.practicals == null) newModule.practicals = module.practicals;
        if (newModule.code == null) newModule.code = module.code;

        //TODO sort this shit out
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("UPDATE Module WHERE ModuleID='" + module.moduleID + 
            "' SET Title='" + newModule.title + ",' Code='" + newModule.code +", Term='" + newModule.term + "'", con);

        Int32 returnID = 0;
        using (con)
        {
            try
            {
                con.Open();
                returnID = (Int32)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }

        return returnID;
    }

    public static Int32 updateSession(Session newSession)
    {
        Session session = getSessionByID(newSession.sessionID);

        //check for unfilled details
        if (newSession.title == null) newSession.title = session.title;
        if (newSession.location == null) newSession.location = session.location;
        if (newSession.date == null) newSession.date = session.date;
        if (newSession.staff == null) newSession.staff = session.staff;

        //TODO sort this shit out
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("UPDATE Session WHERE SessionID='" + session.sessionID +
            "' SET Date='" + newSession.date + ",' Description='" + newSession.title + ", Location='" + newSession.location + ", Staff_Member='" + newSession.staff + "'", con);

        Int32 returnID = 0;
        using (con)
        {
            try
            {
                con.Open();
                returnID = (Int32)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                lastError = e.Message;
            }
        }

        return returnID;
    }
    #endregion
}