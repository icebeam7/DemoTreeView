using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DemoTreeView.Model;
using DemoTreeView.Service;

using Xamarin.Forms;

namespace DemoTreeView
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private bool _IsLoaded;

        protected override void OnAppearing()
        {
            if (_IsLoaded)
            {
                return;
            }

            _IsLoaded = true;

            var xamlItemGroups = DataService.GroupData();

            var rootNodes = ProcessXamlItemGroups(xamlItemGroups);

            foreach (var node in rootNodes)
            {
                var xamlItemGroup = (XamlItemGroup)node.BindingContext;
            }

            TheTreeView.RootNodes = rootNodes;

            base.OnAppearing();
        }

        private static void ProcessXamlItems(TreeViewNode node, XamlItemGroup xamlItemGroup)
        {
            var children = new ObservableCollection<TreeViewNode>();
            foreach (var xamlItem in xamlItemGroup.XamlItems.OrderBy(xi => xi.Key))
            {
                CreateXamlItem(children, xamlItem);
            }
            node.Children = children;
        }

        private static void CreateXamlItem(IList<TreeViewNode> children, XamlItem xamlItem)
        {
            var label = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.Black
            };
            label.SetBinding(Label.TextProperty, "Key");

            var xamlItemTreeViewNode = CreateTreeViewNode(xamlItem, label, true);
            children.Add(xamlItemTreeViewNode);
        }

        private static TreeViewNode CreateTreeViewNode(object bindingContext, Label label, bool isItem)
        {
            var node = new TreeViewNode
            {
                BindingContext = bindingContext,
                Content = new StackLayout
                {
                    Children =
                        {
                            new ResourceImage
                            {
                                Resource = isItem? "DemoTreeView.Resource.Item.png" :"DemoTreeView.Resource.FolderOpen.png" ,
                                HeightRequest= 16,
                                WidthRequest = 16
                            },
                            label
                        },
                    Orientation = StackOrientation.Horizontal
                }
            };

            //set DataTemplate for expand button content
            node.ExpandButtonTemplate = new DataTemplate(() => new ExpandButtonContent { BindingContext = node });

            return node;
        }


        //set what icons shows for expanded/Collapsed/Leafe Nodes or on request node expand icon (when ShowExpandButtonIfEmpty true).
        public class ExpandButtonContent : ContentView
        {

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();

                var node = (BindingContext as TreeViewNode);
                bool isLeafNode = (node.Children == null || node.Children.Count == 0);

                //empty nodes have no icon to expand unless showExpandButtonIfEmpty is et to true which will show the expand
                //icon can click and populated node on demand propably using the expand event.
                if ((isLeafNode) && !node.ShowExpandButtonIfEmpty)
                {
                    Content = new ResourceImage
                    {
                        Resource = isLeafNode ? "DemoTreeView.Resource.Blank.png" : "DemoTreeView.Resource.FolderOpen.png",
                        HeightRequest = 16,
                        WidthRequest = 16
                    };
                }
                else
                {
                    Content = new ResourceImage
                    {
                        Resource = node.IsExpanded ? "DemoTreeView.Resource.OpenGlyph.png" : "DemoTreeView.Resource.CollpsedGlyph.png",
                        HeightRequest = 16,
                        WidthRequest = 16
                    };
                }
            }

        }

        private static ObservableCollection<TreeViewNode> ProcessXamlItemGroups(XamlItemGroup xamlItemGroups)
        {
            var rootNodes = new ObservableCollection<TreeViewNode>();

            foreach (var xamlItemGroup in xamlItemGroups.Children.OrderBy(xig => xig.Name))
            {

                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.Black
                };
                label.SetBinding(Label.TextProperty, "Name");

                var groupTreeViewNode = CreateTreeViewNode(xamlItemGroup, label, false);

                rootNodes.Add(groupTreeViewNode);

                groupTreeViewNode.Children = ProcessXamlItemGroups(xamlItemGroup);

                foreach (var xamlItem in xamlItemGroup.XamlItems)
                {
                    CreateXamlItem(groupTreeViewNode.Children, xamlItem);
                }

            }

            return rootNodes;
        }

        private async void TheTreeView_SelectedItemChanged(object sender, EventArgs e)
        {
            //var selectedItem = TheTreeView.SelectedItem?.BindingContext as Something;
            //if (selectedItem != null)
            //{
            //    await DisplayAlert("Item Selected", $"Selected Content: {selectedItem.TestString}", "OK");
            //}
        }
    }
}
