﻿using CSharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace c08d02_Triangles
{
    public partial class FormMain : Form
    {
        private Scene scene;
        private TrianglesNode triangleNode;
        private ActionList actionList;
        private Picking pickingAction;

        public FormMain()
        {
            InitializeComponent();

            this.Load += FormMain_Load;
            this.winGLCanvas1.OpenGLDraw += winGLCanvas1_OpenGLDraw;
            this.winGLCanvas1.Resize += winGLCanvas1_Resize;
            this.winGLCanvas1.MouseMove += winGLCanvas1_MouseMove;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            var position = new vec3(1, 0.6f, 1) * 16;
            var center = new vec3(0, 0, 0);
            var up = new vec3(0, 1, 0);
            var camera = new Camera(position, center, up, CameraType.Perspecitive, this.winGLCanvas1.Width, this.winGLCanvas1.Height);
            this.scene = new Scene(camera);
            this.scene.RootNode = GetRootNode();

            this.triangleTip = new LegacyTriangleNode();
            this.triangleTip.LineWidth = 10;

            var list = new ActionList();
            list.Add(new TransformAction(scene.RootNode));
            list.Add(new RenderAction(scene));
            this.actionList = list;

            this.pickingAction = new Picking(scene);

            var manipulater = new FirstPerspectiveManipulater();
            manipulater.Bind(camera, this.winGLCanvas1);

        }

        private SceneNodeBase GetRootNode()
        {
            var group = new GroupNode();
            {
                const int length = 5;
                var model = new TrianglesModel(length, length, length);
                var node = TrianglesNode.Create(model, TrianglesModel.strPosition, TrianglesModel.strColor, model.GetSize());
                group.Children.Add(node);

                this.triangleNode = node;
            }
            return group;
        }
        //private SceneNodeBase GetRootNode()
        //{
        //    var group = new GroupNode();
        //    var filenames = new string[] { "floor.obj_", "cube.obj_", };
        //    var colors = new Color[] { Color.Green, Color.White, };
        //    for (int i = 0; i < filenames.Length; i++)
        //    {
        //        string folder = System.Windows.Forms.Application.StartupPath;
        //        string filename = System.IO.Path.Combine(folder, filenames[i]);
        //        var parser = new ObjVNFParser(true);
        //        ObjVNFResult result = parser.Parse(filename);
        //        if (result.Error != null)
        //        {
        //            MessageBox.Show(result.Error.ToString());
        //        }
        //        else
        //        {
        //            ObjVNFMesh mesh = result.Mesh;
        //            var model = new AdjacentTriangleModel(mesh);
        //            var node = PointsNode.Create(model, ObjVNF.strPosition, ObjVNF.strNormal, model.GetSize());
        //            node.Color = colors[i].ToVec3();
        //            node.WorldPosition = new vec3(0, i * 5, 0);
        //            node.Name = filename;
        //            group.Children.Add(node);
        //        }
        //    }

        //    return group;
        //}

        private void winGLCanvas1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            ActionList list = this.actionList;
            if (list != null)
            {
                vec4 clearColor = this.scene.ClearColor;
                GL.Instance.ClearColor(clearColor.x, clearColor.y, clearColor.z, clearColor.w);
                GL.Instance.Clear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);

                list.Act(new ActionParams(Viewport.GetCurrent()));
            }
        }

        void winGLCanvas1_Resize(object sender, EventArgs e)
        {
            this.scene.Camera.AspectRatio = ((float)this.winGLCanvas1.Width) / ((float)this.winGLCanvas1.Height);
        }

        private void rdoRandom_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoRandom.Checked)
            {
                this.triangleNode.Method = TrianglesNode.EMethod.Random;
            }
            else
            {
                this.triangleNode.Method = TrianglesNode.EMethod.gl_VertexID;
            }
        }

        private void rdogl_VertexID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdogl_VertexID.Checked)
            {
                this.triangleNode.Method = TrianglesNode.EMethod.gl_VertexID;
            }
            else
            {
                this.triangleNode.Method = TrianglesNode.EMethod.Random;
            }
        }


    }
}
