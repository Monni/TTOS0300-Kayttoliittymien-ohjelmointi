using MemberRegister;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //testdbEntities dbEnts;
        TietokantaEntityContainer dbEnts;

        // What list to show
        List<jasenet> jasenetToShow;

        ObservableCollection<jasenet> localMembers;
        ICollectionView view; // for filtering datagrid
        bool isMembers; //are the members in datagrid

        public MemberDatabaseConnection MemberDbConn;
        public MainWindow()
        {
            InitializeComponent();
            InitStuff();
            ListMembers();
        }
        private void GetMembers()
        {
            //dgMembers.DataContext = dbEnts.jasenetSet.ToList();
            jasenetToShow = dbEnts.jasenetSet.ToList();
            dgMembers.DataContext = jasenetToShow;
            isMembers = true;
        }

        private void GetAllMembers()
        {
            jasenetToShow = dbEnts.jasenetSet.ToList();
        }
        private void InitStuff()
        {
            dbEnts = new TietokantaEntityContainer(); //Creating a context
            dbEnts.jasenetSet.Load(); //let's load the data locally
            localMembers = dbEnts.jasenetSet.Local;
            dgMembers.DataContext = localMembers.Select(n => n.snimi).Distinct();
            view = CollectionViewSource.GetDefaultView(localMembers);
            try
            {
                MemberDbConn = new MemberDatabaseConnection();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spMidRight.DataContext = dgMembers.SelectedItem;
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string[] info = new string[11];
            info[0] = tbHetu.Text;
            info[1] = tbLastNames.Text;
            info[2] = tbFirstNames.Text;
            info[3] = tbAddrActual.Text;
            info[4] = tbAddrZip.Text;
            info[5] = tbAddrCity.Text;
            info[6] = tbPhnNumber.Text;
            info[7] = tbEmail.Text;
            info[8] = dtpJoinDate.Text;
            info[9] = tbPaidAmount.Text;
            info[10] = tbInfo.Text;

            try
            {
                jasenet jasen = (jasenet)dgMembers.SelectedItems[0];
                int avain = jasen.avain;
                string jasenString = jasen.enimet + " " + jasen.snimi + " (" + jasen.hetu + ") ";


                MemberInfo MI = new MemberInfo(MemberDbConn.connection, CheckConnectionStatus());
                bool success = MI.UpdateMemberInfo(info, avain);
                if (success)
                {
                    MessageBox.Show("Jäsenen " + jasenString + " tiedot päivitetty");
                    ListMembers();
                }

            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Valitse muokattava kohde");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.StackTrace + "\n" + error.Message);
            }

        }

        private void btnAddMember_Click(object sender, RoutedEventArgs e)
        {
            string[] info = new string[11];
            info[0] = tbHetu.Text;
            info[1] = tbLastNames.Text;
            info[2] = tbFirstNames.Text;
            info[3] = tbAddrActual.Text;
            info[4] = tbAddrZip.Text;
            info[5] = tbAddrCity.Text;
            info[6] = tbPhnNumber.Text;
            info[7] = tbEmail.Text;
            info[8] = dtpJoinDate.Text;
            info[9] = tbPaidAmount.Text;
            info[10] = tbInfo.Text;

            string jasenString = info[2] + " " + info[1] + " (" + info[0] + ") ";

            try
            {
                MemberInfo MI = new MemberInfo(MemberDbConn.connection, CheckConnectionStatus());
                bool success = MI.createNewMember(info, CheckConnectionStatus());
                if (success)
                {
                    MessageBox.Show("Jäsen " + jasenString + " lisätty");
                    ListMembers();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.StackTrace + "\n" + error.Message);
            }

        }

        private void btnRemoveMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                jasenet jasen = (jasenet)dgMembers.SelectedItems[0];
                int avain = jasen.avain;
                string jasenpoisto = jasen.enimet + " " + jasen.snimi + " (" + jasen.hetu + ") ";
             
                MemberInfo MI = new MemberInfo(MemberDbConn.connection, CheckConnectionStatus());
                MI.removeMember(avain);
                MessageBox.Show("Jäsen: " + jasenpoisto + " poistettu. ID " + avain);
                ListMembers();
            } catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Valitse poistettava jäsen");
            } catch (Exception error)
            {
                MessageBox.Show(error.StackTrace + "\n" + error.Message);
            }

        }

        private void btnConnectDb_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckConnectionStatus())
            {
                try
                {
                    MemberDbConn.ConnectDatabase();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                CheckConnectionStatus();
            }

        }

        private bool CheckConnectionStatus()
        {

            if (MemberDbConn.CheckConnection())
            {
                tbDbStatus.Text = "Yhteys kunnossa";
                return true;
            }
            else
            {
                tbDbStatus.Text = "Yhteys alhaalla";
                return false;
            }
        }

        private void ListMembers()
        {
            if (CheckConnectionStatus())
            {
                GetMembers();
            }
            else
            {
                try
                {
                    MemberDbConn.ConnectDatabase();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                //list all the members
            }
        }

        private void btnRefreshDb_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckConnectionStatus())
            {
                try
                {
                    MemberDbConn.ConnectDatabase();
                    ListMembers();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                CheckConnectionStatus();
            }
            else if (CheckConnectionStatus())
            {
                ListMembers();
            }

        }

        private void exportToPdf_Click(object sender, RoutedEventArgs e)
        {          
            ExportToPdf exportToPdf = new ExportToPdf();
            Boolean success = exportToPdf.Export(jasenetToShow);
            if (success)
            {
                MessageBox.Show("PDF tallennettu");
            }
        }


        private void showMembers()
        {
            dgMembers.DataContext = jasenetToShow;
        }

        private void btnGetMembersByPostalcode_Click(object sender, RoutedEventArgs e)
        {
            int postalcode = int.Parse(postalBox.Text);
            MemberInfo MI = new MemberInfo(MemberDbConn.connection, CheckConnectionStatus());
            jasenetToShow = MI.GetMembersByPostalcode(postalcode);
            showMembers();
        }

        private void btnGetMembersByFirstName_Click(object sender, RoutedEventArgs e)
        {
            String name = nameSearchBox.Text;
            MemberInfo MI = new MemberInfo(MemberDbConn.connection, CheckConnectionStatus());
            jasenetToShow = MI.getMembersByFirstName(name);
            showMembers();
        }

        private void btnGetMembersByLastName_Click(object sender, RoutedEventArgs e)
        {
            String name = lastnameSearchBox.Text;
            MemberInfo MI = new MemberInfo(MemberDbConn.connection, CheckConnectionStatus());
            jasenetToShow = MI.getMembersByLastName(name);
            showMembers();
        }


        private void exportToMailinglabel_Click(object sender, RoutedEventArgs e)
        {
            ExportToMailinglabel exportToMailinglabel = new ExportToMailinglabel();
            Boolean success = exportToMailinglabel.Export(jasenetToShow);
            if (success)
            {
                MessageBox.Show("Postitustarrat luotu");
            }
        }

        private void btnShowUnpaid_Click(object sender, RoutedEventArgs e)
        {
            MemberInfo MI = new MemberInfo(MemberDbConn.connection, CheckConnectionStatus());
            jasenetToShow = MI.getUnpaidMembers(50);
            showMembers();
        }


    }
}
