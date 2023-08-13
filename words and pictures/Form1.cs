using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace words_and_pictures
{
    public partial class Form1 : Form
    {
        private Dictionary<string, ListViewItem> _namesDictionary;

        public Form1()
        {
            InitializeComponent();
            _namesDictionary = new Dictionary<string, ListViewItem>();
            listView1.MultiSelect = false;
            InitView();
            listBox1.SelectedIndexChanged += OnItemSelected;
        }

        private void InitView()
        {
            var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/WordsPic";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var fileNames = Directory.GetFiles(path);

            List<string> names = new List<string>();
            for (int i = 0; i<fileNames.Length; i++)
            {
                var name = fileNames[i];
                using (var stream = new FileStream(name, FileMode.Open))
                {
                    var image = Image.FromStream(stream);
                    imageList.Images.Add(image);
                    var pathArray = name.Split('\\');
                    name = pathArray[pathArray.Length - 1].Split('.')[0];
                    names.Add(name);
                ListViewItem item = new ListViewItem();
                item.ImageIndex = i;
                listView1.Items.Add(item);
                listBox1.Items.Add(names[i]);
                    _namesDictionary[name] = item;
                }
            }
            listView1.View = View.LargeIcon;
            imageList.ImageSize = new Size(128, 128);
            listView1.LargeImageList = imageList;
            var rand = new Random();
            names = names.OrderBy(a => rand.Next()).ToList();

            for(int number = 0; number < imageList.Images.Count; number++)
            {
            }
        }

        private void OnItemSelected(object o, EventArgs e)
        {
            int nameIndex = listBox1.SelectedIndex;
            try
            {
                var name = listBox1.Items[nameIndex].ToString();
                var imageIndex = listView1.SelectedItems;
                if (imageIndex.Contains(_namesDictionary[name]))
                {
                    foreach (ListViewItem item in imageIndex)
                    {
                        listView1.Items.Remove(item);
                    }
                    listBox1.Items.RemoveAt(nameIndex);
                }
                else
                {
                    listBox1.SelectedIndex = -1;
                    listView1.SelectedIndices.Clear();
                    
                }
            }
            catch { }
        }
    }
}
