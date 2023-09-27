using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Web.Administration;



namespace NewRecycle
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load the XML file and populate the dropdown list
                LoadWebApps();
            }

            // Apply bigger font and bold style to customer names in the dropdown handled by <Customer> tags in XML
            foreach (ListItem item in webAppList.Items)
            {
                if (IsCustomerName(item.Text))
                {
                    item.Attributes["style"] = "font-size: 20px; font-weight: bold;";
                }
            }

            // Set the CSS class for styling the customer names
            webAppList.CssClass = "customer-names";

        }

        protected void RecycleButton_Click(object sender, EventArgs e)
        {

            string selectedWebApp = webAppList.SelectedItem.Text;
            // Log IP address, date, and time
            string logFileName = $"Log_{DateTime.Now:dd-MMM-yyyy}.txt";
            string logFilePath = Server.MapPath($"~/Logs/{logFileName}");
            string publicIPAddress = GetUserIPAddress();
            string logEntry = $"Recycle button executed at -- {DateTime.Now}{Environment.NewLine}IP Address: {publicIPAddress}{Environment.NewLine}";

            // Check if log folder exists
            string logFolder = Server.MapPath("~/Logs");
            if (!Directory.Exists(logFolder))
            {
                //throw new DirectoryNotFoundException("Logs folder not found.");
                //message.InnerText = "Logs folder not found.";
                //message.Style["color"] = "red";
                //create folder in root called "Logs"
                Directory.CreateDirectory(logFolder);
            }
 

            // Write log entry to file
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);


            //using (ServerManager serverManager = new ServerManager())
            //{
            //    ApplicationPool appPool = serverManager.ApplicationPools.FirstOrDefault(pool => pool.Name == selectedWebApp);

            //    if (appPool != null)
            //    {
            //        try
            //        {
            //            appPool.Recycle();
            //            message.InnerText = "Application pool " +selectedWebApp+ " has been recycled successfully.";
            //            message.Style["color"] = "green";
            //        }
            //        catch (Exception ex)
            //        {
            //            message.InnerText = "Failed to recycle the application pool. Reason: " + ex.Message;
            //            message.Style["color"] = "red";
            //        }
            //    }
            //    else
            //    {
            //        message.InnerText = "Selected application pool not found.";
            //        message.Style["color"] = "red";
            //    }
            //}


            string selectedValue = webAppList.SelectedValue;

            try
            {
                using (ServerManager serverManager = new ServerManager())
                {
                    if (IsCustomerName(selectedValue))
                    {
                        bool success = RecycleCustomerApplicationPools(serverManager, selectedValue);

                        if (success)
                        {
                            message.InnerText = "Application pools for " + selectedValue + " have been recycled.";
                            message.Style["color"] = "green";
                        }
                        else
                        {
                            message.InnerText = "Failed to recycle application pools for " + selectedValue + ".";
                            message.Style["color"] = "red";
                        }
                    }
                    else
                    {
                        bool success = RecycleIndividualApplicationPool(serverManager, selectedValue);

                        if (success)
                        {
                            message.InnerText = "Application pool " + selectedValue + " has been recycled.";
                            message.Style["color"] = "green";
                        }
                        else
                        {
                            message.InnerText = "Failed to recycle application pool " + selectedValue + ".";
                            message.Style["color"] = "red";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message.InnerText = "An error occurred: " + ex.Message;
            }



        }



        private void LoadWebApps()
        {
            //string xmlFile = Server.MapPath("~/webApps.xml");
            //XDocument doc = XDocument.Load(xmlFile);
            //var webApps = doc.Descendants("Name");

            //foreach (var webApp in webApps)
            //{
            //    string webAppName = webApp.Value;
            //    if (!string.IsNullOrEmpty(webAppName))
            //    {
            //        webAppList.Items.Add(new ListItem(webAppName));
            //    }
            //}

            string xmlFilePath = Server.MapPath("~/webApps.xml");

            // Load the XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            // Retrieve the customer data from the XML
            XmlNodeList customerNodes = xmlDoc.SelectNodes("//Customer");

            // Loop through each customer node and retrieve the customer name
            foreach (XmlNode customerNode in customerNodes)
            {
                string customerName = customerNode.Attributes["Name"].Value;
                XmlNodeList appPoolNodes = customerNode.SelectNodes("WebApplication");

                // Add the customer name as an option in the dropdown
                ListItem customerItem = new ListItem(customerName, customerName);
                webAppList.Items.Add(customerItem);

                // Loop through each application pool and add it to the dropdown
                foreach (XmlNode appPoolNode in appPoolNodes)
                {
                    string appPoolName = appPoolNode.InnerText;

                    // Add the individual application pool as an option in the dropdown
                    ListItem appPoolItem = new ListItem(appPoolName, appPoolName);
                    webAppList.Items.Add(appPoolItem);
                }
            }






        }


        private bool IsCustomerName(string value)
        {
            // Implement your logic to determine if the value represents a customer name
            // For example, you can check if the value exists in the XML under the 'Name' attribute of 'Customer' nodes
            // Return true if it represents a customer name, false otherwise
            // Replace this placeholder implementation with your actual logic
            return value.StartsWith("Customer");
        }


        private bool RecycleCustomerApplicationPools(ServerManager serverManager, string customerName)
        {
            // Retrieve the customer node from the XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("~/webApps.xml"));
            XmlNode customerNode = xmlDoc.SelectSingleNode("//Customer[@Name='" + customerName + "']");


            if (customerNode != null)
            {
                // Retrieve the application pools for the selected customer
                XmlNodeList appPoolNodes = customerNode.SelectNodes("WebApplication");

                // Recycle each application pool for the customer
                foreach (XmlNode appPoolNode in appPoolNodes)
                {
                    string appPoolName = appPoolNode.InnerText;

                    // Recycle the application pool using the appPoolName
                    bool success = RecycleApplicationPool(serverManager, appPoolName);

                    if (!success)
                    {
                        // Error occurred while recycling the application pool
                        return false;
                    }
                }

                // All application pools recycled successfully
                return true;
            }

            // Customer not found in the XML
            return false;
        }

        private bool RecycleApplicationPool(ServerManager serverManager, string appPoolName)
        {
            try
            {
                // Recycle the individual application pool
                ApplicationPool appPool = serverManager.ApplicationPools.FirstOrDefault(ap => ap.Name.Equals(appPoolName));

                // Recycle the application pool if found
                if (appPool != null)
                {
                    appPool.Recycle();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                // Display error message on the front end or log the error
                message.InnerText = "Error occurred while recycling the application pool: " + ex.Message;
            }

            // Application pool not found or error occurred
            return false;
        }

        private bool RecycleIndividualApplicationPool(ServerManager serverManager, string appPoolName)
        {
            try
            {
                // Recycle the individual application pool
                ApplicationPool appPool = serverManager.ApplicationPools.FirstOrDefault(ap => ap.Name.Equals(appPoolName));

                // Recycle the individual application pool if found
                if (appPool != null)
                {
                    appPool.Recycle();
                    return true;
                }
                else
                {
                    // Application pool not found
                    message.InnerText = "Application pool " + appPoolName + " not found.";
                    message.Style["color"] = "red";
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                // Display error message on the front end or log the error
                message.InnerText = "Error occurred while recycling the application pool: " + ex.Message;
            }

            // Error occurred
            return false;
        }



        private string GetUserIPAddress()
        {
            string userIPAddress = string.Empty;

            string forwardedFor = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                string[] ipArray = forwardedFor.Split(',');
                userIPAddress = ipArray[0].Trim();
            }
            else
            {
                userIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return userIPAddress;
        }





    }
}