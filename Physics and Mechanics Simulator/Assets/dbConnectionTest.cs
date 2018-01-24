using UnityEngine;
using System.Collections;
using Mono.Data.SqliteClient;
using System.Data;

public class dbConnectionTest : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		string conn = "URI=file:" + Application.dataPath + "/unity_db"; //Path to database.
		Debug.Log ("Path is : "+conn);
		IDbConnection dbconn;
		dbconn = (IDbConnection)new SqliteConnection (conn);
		dbconn.Open (); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand ();
		string sqlQuery = "SELECT id " + "FROM users";
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader ();
		while (reader.Read ()) {
			int value = reader.GetInt32 (0);
			Debug.Log ("value= " + value);
		}
		reader.Close ();
		reader = null;
		dbcmd.Dispose ();
		dbcmd = null;
		dbconn.Close ();
		dbconn = null;


	}

	void	getValues ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
