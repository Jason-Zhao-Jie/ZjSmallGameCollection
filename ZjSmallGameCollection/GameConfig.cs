using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ZjSmallGameCollection
{
    public partial class GameConfig : Form
    {
        private static readonly string cfgFile = System.Environment.CurrentDirectory + "\\gameconfig.xml";
        public const string nowVersion = "0.1";
        XmlElement cfgXml;
        public GameConfig()
        {
            InitializeComponent();
        }
        public static XmlElement GetCfgXml()
        {
            var cfgXml = new ConfigXmlDocument();
            try
            {
                cfgXml.Load(cfgFile);
                if(cfgXml.DocumentElement["version"].InnerText != nowVersion)
                {
                    System.IO.File.Delete(cfgFile);
                    throw new System.IO.FileNotFoundException();
                }
            }
            catch(System.IO.FileNotFoundException)
            {
                cfgXml.LoadXml("<?xml version='1.0' encoding='utf-8' ?><gamecfg><version>" + nowVersion + "</version><GameRiver><isWhiteFirst>0</isWhiteFirst><player1><type>auto</type><name>匿名玩家</name><AIdt>1000</AIdt><server>false</server><port>8047</port><ip>192.168.1.101</ip></player1><player2><type>local</type><name>匿名玩家</name><AIdt>1000</AIdt><server>false</server><port>8047</port><ip>192.168.1.101</ip></player2></GameRiver></gamecfg>");
            }
            return cfgXml.DocumentElement;
        }
        public static DialogResult ShowDialog(Form parent)
        {
            var res = new GameConfig().ShowDialog((IWin32Window)parent);
            return res;
        }
        private void GameConfig_Load(object sender, EventArgs e)
        {
            cfgXml = GetCfgXml();
            LoadCfg(cfgXml);
        }
        private void LoadCfg(XmlElement cfgXml)
        {
            var gamerivercfg = cfgXml["GameRiver"];
            var gameriver_p1 = gamerivercfg["player1"];
            var gameriver_p2 = gamerivercfg["player2"];
            player_first.SelectedIndex = Convert.ToInt32(gamerivercfg["isWhiteFirst"].InnerText);

            switch(gameriver_p1["type"].InnerText)
            {
                case "local":
                    player1_local.Select();
                    break;
                case "auto":
                    player1_auto.Select();
                    break;
                case "network":
                    player1_network.Select();
                    break;
            }
            player1_name.Text = gameriver_p1["name"].InnerText;
            player1_AIdt.Text = gameriver_p1["AIdt"].InnerText;
            player1_server.CheckState = Convert.ToBoolean(gameriver_p1["server"].InnerText) ? CheckState.Checked : CheckState.Unchecked;
            player1_port.Text = gameriver_p1["port"].InnerText;
            player1_ip.Text = gameriver_p1["ip"].InnerText;

            switch(gameriver_p2["type"].InnerText)
            {
                case "local":
                    player2_local.Select();
                    break;
                case "auto":
                    player2_auto.Select();
                    break;
                case "network":
                    player2_network.Select();
                    break;
            }
            player2_name.Text = gameriver_p2["name"].InnerText;
            player2_AIdt.Text = gameriver_p2["AIdt"].InnerText;
            player2_server.CheckState = Convert.ToBoolean(gameriver_p2["server"].InnerText) ? CheckState.Checked : CheckState.Unchecked;
            player2_port.Text = gameriver_p2["port"].InnerText;
            player2_ip.Text = gameriver_p2["ip"].InnerText;
        }

        private void Radiobtn1_CheckedChanged(object sender, EventArgs e)
        {
            var btn = (RadioButton)sender;
            switch(btn.Name)
            {
                case "player1_local":
                    player1_name_label.Enabled = btn.Checked;
                    player1_name.Enabled = btn.Checked;
                    break;
                case "player1_auto":
                    player1_AIdt_label.Enabled = btn.Checked;
                    player1_AIdt.Enabled = btn.Checked;
                    break;
                case "player1_network":
                    player1_server.Enabled = btn.Checked;
                    player1_ip.Enabled = btn.Checked;
                    break;
            }
        }

        private void Radiobtn2_CheckedChanged(object sender, EventArgs e)
        {
            var btn = (RadioButton)sender;
            switch(btn.Name)
            {
                case "player2_local":
                    player2_name_label.Enabled = btn.Checked;
                    player2_name.Enabled = btn.Checked;
                    break;
                case "player2_auto":
                    player2_AIdt_label.Enabled = btn.Checked;
                    player2_AIdt.Enabled = btn.Checked;
                    break;
                case "player2_network":
                    player2_server.Enabled = btn.Checked;
                    player2_ip.Enabled = btn.Checked;
                    break;
            }
        }

        private void server_CheckStateChanged(object sender, EventArgs e)
        {
            var btn = (CheckBox)sender;
            switch(btn.Name)
            {
                case "player1_server":
                    player1_port.Enabled = (btn.CheckState == CheckState.Checked);
                    player1_port_label.Enabled = (btn.CheckState == CheckState.Checked);
                    if(btn.CheckState == CheckState.Checked)
                        player2_server.CheckState = CheckState.Unchecked;
                    player2_server.Enabled = (btn.CheckState != CheckState.Checked);
                    break;
                case "player2_server":
                    player2_port.Enabled = (btn.CheckState == CheckState.Checked);
                    player2_port_label.Enabled = (btn.CheckState == CheckState.Checked);
                    if(btn.CheckState == CheckState.Checked)
                        player1_server.CheckState = CheckState.Unchecked;
                    player1_server.Enabled = (btn.CheckState != CheckState.Checked);
                    break;
            }
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void usebtn_Click(object sender, EventArgs e)
        {
            var gamerivercfg = cfgXml["GameRiver"];
            var gameriver_p1 = gamerivercfg["player1"];
            var gameriver_p2 = gamerivercfg["player2"];
            gamerivercfg["isWhiteFirst"].InnerText = player_first.SelectedIndex.ToString();

            if(player1_local.Checked)
                gameriver_p1["type"].InnerText = "local";
            else if(player1_auto.Checked)
                gameriver_p1["type"].InnerText = "auto";
            else
                gameriver_p1["type"].InnerText = "network";
            gameriver_p1["name"].InnerText = player1_name.Text;
            gameriver_p1["AIdt"].InnerText = player1_AIdt.Text;
            gameriver_p1["server"].InnerText = (player1_server.CheckState == CheckState.Checked) ? "true" : "false";
            gameriver_p1["port"].InnerText = player1_port.Text;
            gameriver_p1["ip"].InnerText = player1_ip.Text;

            if(player2_local.Checked)
                gameriver_p2["type"].InnerText = "local";
            else if(player2_auto.Checked)
                gameriver_p2["type"].InnerText = "auto";
            else
                gameriver_p2["type"].InnerText = "network";
            gameriver_p2["name"].InnerText = player2_name.Text;
            gameriver_p2["AIdt"].InnerText = player2_AIdt.Text;
            gameriver_p2["server"].InnerText = (player2_server.CheckState == CheckState.Checked) ? "true" : "false";
            gameriver_p2["port"].InnerText = player2_port.Text;
            gameriver_p2["ip"].InnerText = player2_ip.Text;

            cfgXml.OwnerDocument.Save(cfgFile);
        }

        private void okbtn_Click(object sender, EventArgs e)
        {
            usebtn_Click(sender, e);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
