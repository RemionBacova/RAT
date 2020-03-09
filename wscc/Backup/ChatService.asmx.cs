using System.Web.Services;
using System.Collections;

namespace wscc
{
    /// <summary>
    /// Summary description for ChatService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ChatService : System.Web.Services.WebService
    {

        static protected ArrayList arrUsers = new ArrayList();
        static protected ArrayList arrMessage = new ArrayList();

        public ChatService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent();        
        }
        [WebMethod]
        public string GetUsers()
        {
            string strUser = string.Empty;
            for (int i = 0; i < arrUsers.Count; i++)
            {
                strUser = strUser + arrUsers[i].ToString() + "|";
            }
            return strUser;
        }

        [WebMethod]
        public void AddUser(string strUser)
        {
            bool bFlag = false;
            for (int i = 0; i < arrUsers.Count; i++)
            {
                if (arrUsers[i].ToString() == strUser)
                    bFlag = true;
                else
                    SendMessage("Server", arrUsers[i].ToString(), strUser + " is Connected.");
            }
            if (bFlag == false)
                arrUsers.Add(strUser);
        }

        [WebMethod]
        public void RemoveUser(string strUser)
        {
            for (int i = 0; i < arrUsers.Count; i++)
            {
                if (arrUsers[i].ToString() == strUser)
                    arrUsers.RemoveAt(i);
            }
        }

        [WebMethod]
        public void SendMessage(string strFromUser, string strToUser, string strMess)
        {
            arrMessage.Add(strToUser + ":" + strFromUser + ":" + strMess);
        }

        [WebMethod]
        public string ReceiveMessage(string strUser)
        {
            string strMess = string.Empty;
            for (int i = 0; i < arrMessage.Count; i++)
            {
                string[] strTo = arrMessage[i].ToString().Split(':');
                if (strTo[0].ToString() == strUser)
                {
                    for (int j = 1; j < strTo.Length; j++)
                    {
                        strMess = strMess + strTo[j] + ":";
                    }
                    arrMessage.RemoveAt(i);
                    break;
                }
            }
            return strMess;
        }
    }
}
