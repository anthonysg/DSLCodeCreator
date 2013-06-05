using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Eto.Drawing;
using Eto.Forms;
using Eto.Xaml;

using GitSharp;
using System.Collections.Generic;



namespace CodeCreatorDSL
{
    class MainClass : Application
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Application();

            app.Initialized += delegate
            {
                app.MainForm = new MyForm();
                app.MainForm.Show();
            };
            app.Run(args);

        }

        class MyForm : Form
        {
            /*
          int x;
            public int X
            {
                get {
                  return x;  
                }

                set {
                    if (0 < value) {
                        x = value;
                    } else {
                        throw new InvalidOperationException("");
                    }
                }
            }
            
            public int Y { get; set; }
            public int Z { get; private set; }

            event EventHandler<EventArgs> Click;
                
            public string Password 

    
            string hash;
            public string Password { 
                get {
                    return hash;
                }
                set {
                    hash = sha1(value);
                }
            }
            
            verify();

*/
            Button btn_exit;
            TextArea txtbox_Feed;
            StreamReader streamOfText;

            public MyForm()
            {
                /*this.X = 4;
                int z = this.X + 3;

                var d = delegate { };
                var c = delegate { };
                this.Click += d;
                this.Click -= d;
*/

                this.ClientSize = new Size(400, 350);
                this.WindowState = WindowState.Normal;
                this.Title = "DSLMi/o Code Creator";
                

                //var btn = new Button() { Text = "Real button" };
                var layout = new DynamicLayout(this);
                layout.Spacing = new Size(6, 6);
                layout.AddRow(new Label { Text = "DSLMi/o Code Creator" });
                var btn_save = new Button { Text = "Save" };
                var progress_bar = new ProgressBar();
                layout.AddRow(btn_save, progress_bar); //, new Panel();
                Application.Instance.Invoke(() => { progress_bar.Value = 100; });
                var btn_open = new Button { Text = "Open" };
                layout.AddRow(btn_open);
                Button btn_git = new Button{ Text = "Git Implementation" };
                layout.AddRow(btn_git);
                btn_git.Click += OnBtnGitClicked;
                
                btn_exit = new Button { Text = "Exit"  };
                btn_exit.Click += OnBtnExitClick;
                this.Closing += OnApplicationClosing;
                // or if with code at bottom: btn_exit.Click += OnBtnExitClick { Close(); };
                layout.AddRow(btn_exit);
                var list = new ListBox();
                //var fonts = Fonts.AvailableFontFamilies();
                txtbox_Feed = new TextArea();
                try
                {
                    //txtbox_Feed.Font = new Font("Helvetica, Arial, serif, sans-serif", 10);
                    txtbox_Feed.Font = new Font("Helvetica, Arial, serif, sans-serif", 10);
                } catch (Exception ex) {
                    txtbox_Feed.Font = new Font(SystemFont.Default, 10);
                }

                btn_save.Click += OnBtnSaveClick;
                layout.AddRow(list, txtbox_Feed);
                btn_open.Click += OnBtnOpenClick;

                
                
            }

                /*  btn_exit.Click += OnBtnExitClick;
                  btn_exit.Click += delegate { Close(); };
                  btn_exit.Click += (s, e) =>
                  {

                  };

                  btn_save.Click += (s, e) =>
                  {
                        
                  };
                
              //myButton.Click += (s,e) => { MessageBox.Show("hello there"); */
            

          /*  void OnBtnExitClick(object sender, EventArgs e)
            {
                Close();
            } This is one way to cause an event-based click better in scenarios where there are multiple clicks (references code nearby btn_exit) */


            void OnBtnSaveClick(object sender, EventArgs e)
            {
                
                SaveFileDialog savefiledialog = new SaveFileDialog();

                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                Uri defaultUri = new Uri(path);
                savefiledialog.Directory = defaultUri;

                string defaultFileName = "Automatically Generated DSL Codes";

                string[] filePaths = Directory.GetFiles(path);

                string newFilePaths = string.Join(" ", filePaths);
                
                MatchCollection matches = Regex.Matches(newFilePaths, "Automatically Generated DSL Codes");

                int firstfile = matches.Count;

                firstfile++;

                string finalDefaultFileName = defaultFileName + " " + firstfile;


                savefiledialog.Filters = new FileDialogFilter[] { new FileDialogFilter("C#", ".cs"), new FileDialogFilter("Text Files", ".txt") };
                

                savefiledialog.FileName = finalDefaultFileName;
                
                if (savefiledialog.ShowDialog(ParentWindow) == DialogResult.Ok)
                {
                    File.WriteAllText(savefiledialog.FileName, txtbox_Feed.Text);
                    
                        //Odd glitch from code below was not solved.
                        /*var enc = new UTF8Encoding();
                        
                        var buffer = enc.GetBytes(txtbox_Feed.Text);
                        var str = buffer.ToString();
                        stream.Write(buffer, 0, buffer.Length);
                        stream.Flush(); */
                    
                    
                }
                
                
            }

            void OnBtnOpenClick(object sender, EventArgs e)
            {
                OpenFileDialog openfiledialog = new OpenFileDialog();

                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                Uri openDirectory = new Uri(folderPath);
                openfiledialog.Directory = openDirectory;

                openfiledialog.Filters = new FileDialogFilter[] { new FileDialogFilter("C#", ".cs"), new FileDialogFilter("Text Files", ".txt") };
                
                

                if (openfiledialog.ShowDialog(ParentWindow) == DialogResult.Ok)
                {
                    streamOfText = new StreamReader(openfiledialog.FileName);

                    var text = streamOfText.ReadToEnd();
                    streamOfText.Close();

                    txtbox_Feed.Text = text;
                    
                }
            }

            void OnBtnExitClick(object sender, EventArgs e)
            {
                Application.Instance.Quit();
            }

            void OnApplicationClosing(object sender, System.ComponentModel.CancelEventArgs e)
            {     
                Dialog confirm = new Dialog();
                confirm.Title = "Confirm";

                var layout = new DynamicLayout(confirm, new Padding(20, 5), new Size(10, 10));
                layout.Add(new Label { Text = "Do You Want to Save Changes?", Font = new Font(SystemFont.Bold, 8), HorizontalAlign = HorizontalAlign.Center });

                confirm.DefaultButton = new Button { Text = "No" };
                confirm.AbortButton = new Button { Text = "Cancel" };
                Button yes_DoSave = new Button { Text = "Yes" };
                
                confirm.DefaultButton.Click += delegate { 
                    confirm.Close();
                    confirm.DialogResult = DialogResult.Yes;
                };
                
                confirm.AbortButton.Click += delegate
                {
                    confirm.DialogResult = DialogResult.Abort;
                };

                yes_DoSave.Click += delegate
                {
                    confirm.Close();
                    OnBtnSaveClick(sender, e);
                };

                layout.AddCentered(confirm.DefaultButton);
                layout.AddCentered(confirm.AbortButton);
                layout.AddCentered(yes_DoSave);

                confirm.WindowState = WindowState.Normal;

                confirm.Closed += delegate
                {
                    confirm.DialogResult = DialogResult.Abort;
                };

                var result = confirm.ShowDialog(Parent);
                if (DialogResult.Abort == result)
                    e.Cancel = true;
            }

            void OnBtnGitClicked(object s, EventArgs e)
            {
                Form git_confirm = new Form();
                git_confirm.Title = "Git Functionality";

                var layout = new DynamicLayout(git_confirm, new Padding(20, 5), new Size(10, 10));
                layout.Add(new Label { Text = "Specify the Following Git Settings.", Font = new Font(SystemFont.Bold, 8), HorizontalAlign = HorizontalAlign.Center });

                TextBox git_txt_box = new TextBox();
                Button git_btn_ok = new Button { Text = "OK" };
                ListBox git_display = new ListBox();

                layout.AddCentered(git_txt_box);
                layout.AddCentered(git_btn_ok);
                layout.Add(git_display);

                git_btn_ok.Click += delegate
                {
                    //Git Repo Test
                    string directory = git_txt_box.Text;
                    
                    string repository = Repository.FindRepository(directory);

                    Repository repo;
                    IEnumerable<string> gitFileList = new HashSet<string>();
                    if (Repository.IsValid(repository))
                    {
                        repo = new Repository(repository);

                        gitFileList = repo.Index.Entries;

                        git_display.Items.Clear();

                        //Display Untracked/Uncommitted Files
                        foreach (var item in repo.Index.Status.Untracked)
                        {
                            git_display.Items.Add(item);
                        }

                        //Display Tracked/Committed Files
                        foreach (var item in gitFileList)
                        {
                            git_display.Items.Add(item);
                        }

                        git_display.Items.Add(repo.Index.Status.Untracked.ToString());
                    }
                };

                git_confirm.Show();
                
            }
        }
        //Fix Git glitch for My Documents folder- likely an issue related to the branch.
        //Also change the "are you sure" exit dialog so that it asks if you want to save first. a save dialog will appear if a file is currently open and not been saved.
        //Make filedialogfilters contain both .txt and .cs in the same type by default (same dropdown menu option)
        //Make the .cs file that is generated, properly get referenced (possible?)
        //Have the .cs file interact with git.
        //Organize Code into different .cs files
        //Add ToolBar
        //Start populating Listbox with flowchart-esque organization.
        //Go back to load bar later on.
        //look into linux/mac compatibility for filename (specifying default My Documents directory)
    }
}

