﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuantumChess.App.Model;

namespace QuantumChess.App.Controls
{
	/// <summary>
	/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
	///
	/// Step 1a) Using this custom control in a XAML file that exists in the current project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:QuantumChess.App.Controls"
	///
	///
	/// Step 1b) Using this custom control in a XAML file that exists in a different project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:QuantumChess.App.Controls;assembly=QuantumChess.App.Controls"
	///
	/// You will also need to add a project reference from the project where the XAML file lives
	/// to this project and Rebuild to avoid compilation errors:
	///
	///     Right click on the target project in the Solution Explorer and
	///     "Add Reference"->"Projects"->[Browse to and select this project]
	///
	///
	/// Step 2)
	/// Go ahead and use your control in the XAML file.
	///
	///     <MyNamespace:BoardView/>
	///
	/// </summary>
	public class BoardView : Control
	{
		public static readonly DependencyProperty BoardSpaceProperty = DependencyProperty.Register(
			"BoardSpace", typeof(BoardSpace), typeof(BoardView), new PropertyMetadata(default(BoardSpace)));

		public BoardSpace BoardSpace
		{
			get { return (BoardSpace) GetValue(BoardSpaceProperty); }
			set { SetValue(BoardSpaceProperty, value); }
		}

		static BoardView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(BoardView), new FrameworkPropertyMetadata(typeof(BoardView)));
		}
	}
}
