<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newrecycle.aspx.cs" Inherits="NewRecycle.WebForm1" %>
<!DOCTYPE html>
<html>

<head runat="server">

    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Recycle IIS Web Applications</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
            text-align: center;
        }

        h1 {
            margin-bottom: 20px;
            color: #333;
        }

        .container {
            display: flex;
            flex-direction: column;
            align-items: center;
            max-width: 400px;
            margin: 0 auto;
            background-color: #f2f2f2;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
        }

        .container label {
            margin-bottom: 10px;
            font-weight: bold;
            color: #333;
        }

        .container select {
            width: 100%;
            padding: 8px;
            margin-bottom: 10px;
            border-radius: 5px;
            border: 1px solid #ccc;
            box-sizing: border-box;
            font-size: 16px;
        }

        .container button {
            padding: 10px 20px;
            background-color: #4CAF50;
            color: #fff;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

        #message {
            margin-top: 20px;
            font-weight: bold;
            color: #333;
        }

        .animation-container {
            width: 100%;
            height: 100%;
            position: absolute;
            top: 0;
            left: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 999;
            opacity: 0;
            transition: opacity 0.3s ease-in-out;
            pointer-events: none;
        }

        .animation-text {
            color: white;
            font-size: 24px;
        }

        .animation-fade-in {
            opacity: 1;
        }

        .animation-fade-out {
            opacity: 0;
        }

        .customer-names {
        font-size: 20px;
        font-weight: bold;
        }

        .logo {
            display: flex;
            justify-content: center;
            margin-bottom: 20px;
        }
        
        .logo img {
            max-width: 30%;
            height: auto;
        }

        @media screen and (max-width: 480px) {
            /* Mobile styles */
            .container {
                max-width: 100%;
                padding: 10px;
            }

            .container select {
                font-size: 14px;
            }

            .container button {
                padding: 8px 16px;
                font-size: 14px;
            }
        }
    </style>

    <script>
        function showAnimation() {
            var animationContainer = document.getElementById('animationContainer');
            animationContainer.classList.add('animation-fade-in');
            animationContainer.classList.remove('animation-fade-out');

            setTimeout(function () {
                hideAnimation();
            }, 2000); // Hide the animation after 2 seconds
        }

        function hideAnimation() {
            var animationContainer = document.getElementById('animationContainer');
            animationContainer.classList.remove('animation-fade-in');
            animationContainer.classList.add('animation-fade-out');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="logo">
            <a href="newrecycle.aspx"><img src="IIS_logo.png" alt="IIS Logo" /></a>
        </div>

        <h1>Recycle IIS Web Applications</h1>
        
        <div class="container">
            <asp:Label runat="server" AssociatedControlID="webAppList">Select an IIS Web Application:</asp:Label>
            <div class="animation-container" id="animationContainer">
                <div class="animation-text">Processing...</div>
            </div>
            <asp:DropDownList ID="webAppList" runat="server" CssClass="form-control" >
                <asp:ListItem Value="" Disabled Selected>Select an option</asp:ListItem>
            </asp:DropDownList>
            
            <asp:Button ID="recycleButton" runat="server" Text="Recycle" OnClick="RecycleButton_Click" CssClass="btn-primary" OnClientClick="showAnimation();" />
            <div id="message" runat="server"></div>
        </div>
    </form>
</body>
</html>
