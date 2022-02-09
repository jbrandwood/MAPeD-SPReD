﻿/*
 * Created by SharpDevelop.
 * User: 0x8BitDev Copyright 2017-2022 ( MIT license. See LICENSE.txt )
 * Date: 13.09.2018
 * Time: 17:59
 */
 
//#define DEF_DBG_PPU_READY_DATA_SAVE_IMG
 
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace MAPeD
{
	/// <summary>
	/// Description of exporter_pce_asm.
	/// </summary>
	/// 

	public partial class exporter_asm : Form
	{
		private readonly data_sets_manager m_data_mngr = null;
		
		private string	m_filename			= null;
		private string	m_path				= null;
		private string 	m_path_filename_ext	= null;
		private string 	m_path_filename		= null;
		
		private string	m_level_prefix		= null;
		
		private const string	CONST_FILENAME_LEVEL_PREFIX		= "Lev";
		private const string	CONST_BIN_EXT					= ".bin";
		
		private int  m_VDP_ready_scr_data_size = -1;

		private int[] m_CHR_offset	= { 32, 64, 128, 64, 128, 256 };
		private int[] m_BAT_index	= { 0, 1, 3, 4, 5, 7 };
		
		private StreamWriter	m_C_writer	= null;
		
		private struct exp_scr_data
		{
			public int 			m_scr_ind;
			
			public tiles_data 	m_tiles;
			
			public screen_data	m_scr_tiles;
			public screen_data	m_scr_blocks;
			
			public int			m_tiles_offset;
			public int			m_blocks_offset;			
			
			public int			m_VDC_scr_offset;
			
			public static int	_tiles_offset;
			public static int	_blocks_offset;
			
			public exp_scr_data( int _scr_ind, tiles_data _tiles )
			{
				m_scr_ind		= _scr_ind;
				
				m_tiles 		= _tiles;
				
				m_scr_tiles		= new screen_data( data_sets_manager.EScreenDataType.sdt_Tiles4x4 );
				m_scr_blocks	= new screen_data( data_sets_manager.EScreenDataType.sdt_Blocks2x2 );
				
				m_tiles_offset	= _tiles_offset;
				m_blocks_offset	= _blocks_offset;
				
				_tiles_offset 	+= m_scr_tiles.length;
				_blocks_offset 	+= m_scr_blocks.length;
				
				m_VDC_scr_offset = 0;
			}
			
			public void destroy()
			{
				m_tiles 		= null;
				
				m_scr_tiles 	= null;
				m_scr_blocks 	= null;
			}
		};	
		
		public exporter_asm( data_sets_manager _data_mngr )
		{
			m_data_mngr = _data_mngr;
			
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			RBtnLayoutMatrix.Enabled = RBtnLayoutAdjacentScreenIndices.Enabled = RBtnLayoutAdjacentScreens.Enabled = false;
			
			// 64x32 by default, cause its more suitable for scrolling maps
			ComboBoxBAT.SelectedIndex = 1;
		}

		void CheckBoxExportEntitiesChanged_Event(object sender, EventArgs e)
		{
			bool enabled = ( sender as CheckBox ).Checked;
			
			groupBoxEntityCoordinates.Enabled = enabled;
			
			CheckBoxExportMarks.Enabled = ( enabled == false && RBtnModeMultidirScroll.Checked ) ? false:true;
			CheckBoxExportMarks.Checked = CheckBoxExportMarks.Enabled ? CheckBoxExportMarks.Checked:false;
			
			update_desc();
		}

		void RBtnModeMultidirScrollChanged_Event(object sender, EventArgs e)
		{
			RBtnLayoutMatrix.Enabled = RBtnLayoutAdjacentScreenIndices.Enabled = RBtnLayoutAdjacentScreens.Enabled = false;
			RBtnLayoutMatrix.Checked 	= true;
			
			CheckBoxExportMarks.Enabled = ( CheckBoxExportEntities.Checked == false && RBtnModeMultidirScroll.Checked ) ? false:true;
			CheckBoxExportMarks.Checked = CheckBoxExportMarks.Enabled ? CheckBoxExportMarks.Checked:false;
			
			CheckBoxRLE.Enabled = true;
			
			update_desc();
		}
		
		void RBtnModeScreenToScreenChanged_Event(object sender, EventArgs e)
		{
			RBtnLayoutMatrix.Enabled = RBtnLayoutAdjacentScreenIndices.Enabled = RBtnLayoutAdjacentScreens.Enabled = true;
			RBtnLayoutAdjacentScreens.Checked	= true;
			
			CheckBoxExportMarks.Enabled = true;
			
			CheckBoxRLE.Checked = CheckBoxRLE.Enabled = false;
			
			update_desc();
		}

		void RBtnModeStaticScreensChanged_Event(object sender, EventArgs e)
		{
			RBtnLayoutMatrix.Enabled = RBtnLayoutAdjacentScreenIndices.Enabled = RBtnLayoutAdjacentScreens.Enabled = true;
			RBtnLayoutAdjacentScreens.Checked	= true;
			
			CheckBoxExportMarks.Enabled = true;
			
			CheckBoxRLE.Enabled = true;
			
			update_desc();
		}

		void ParamChanged_Event(object sender, EventArgs e)
		{
			update_desc();
		}
		
		void update_desc()
		{
			if( RBtnTiles2x2.Checked )
			{
				RichTextBoxExportDesc.Text = strings.CONST_STR_EXP_TILES_2X2;
			}
			else
			{
				RichTextBoxExportDesc.Text = strings.CONST_STR_EXP_TILES_4X4;
			}
			
			if( CheckBoxRLE.Checked )
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_RLE_COMPRESSION;
			}
			
			RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_DATA_ORDER;
			
			if( RBtnTilesDirColumns.Checked )
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_PCE_DATA_ORDER_COLS;
			}
			else
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_DATA_ORDER_ROWS;
			}

			RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_MODE;
			
			if( RBtnModeMultidirScroll.Checked )
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_MODE_MULTIDIR;
			}
			else
			if( RBtnModeBidirScroll.Checked )
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_MODE_BIDIR;
			}
			else
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_MODE_STAT_SCR.Replace( "<data>", "VDC-ready - " + m_VDP_ready_scr_data_size.ToString() );
			}
			
			RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_PROP;
			
			if( RBtnPropPerBlock.Checked )
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_PROP_PER_BLOCK;
			}
			else
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_PROP_PER_CHR;
			}
			
			RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_LAYOUT;
			
			if( RBtnLayoutAdjacentScreens.Checked )
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_LAYOUT_ADJ_SCR;
			}
			else
			if( RBtnLayoutAdjacentScreenIndices.Checked )
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_LAYOUT_ADJ_SCR_INDS;
			}
			else
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_LAYOUT_MATRIX;
			}
			
			if( CheckBoxExportMarks.Checked )
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_MARKS;
			}
			else
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_NO_MARKS;
			}
			
			if( CheckBoxExportEntities.Checked )
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_ENTITIES;
				
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_ENT_COORDS;
				
				if( RBtnEntityCoordScreen.Checked )
				{
					RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_ENT_COORDS_SCR;
				}
				else
				{
					RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_ENT_COORDS_MAP;
				}				
			}
			else
			{
				RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_NO_ENTITIES;
			}
			
			RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_PCE_CHR_OFFSET;
			
			RichTextBoxExportDesc.Text += strings.CONST_STR_EXP_WARNING;
		}

		void event_cancel(object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		private string get_exp_prefix()
		{
			return CheckBoxGenerateHFile.Checked ? "_":"";
		}
		
		public static string skip_exp_pref( string _str )
		{
			int offs = _str.IndexOf( "_" );
			offs = ( offs == 0 ) ? ( offs + 1 ):0;
			
			return _str.Substring( offs );
		}
		
		public void ShowDialog( string _full_path )
		{
			m_path_filename_ext = _full_path;
			
			if( m_data_mngr.screen_data_type == data_sets_manager.EScreenDataType.sdt_Blocks2x2 )
			{
				RBtnTiles2x2.Checked = true;
				RBtnTiles2x2.Enabled = RBtnTiles4x4.Enabled = false;
			}
			else
			{
				RBtnTiles2x2.Enabled = RBtnTiles4x4.Enabled = true;
			}
			
			m_VDP_ready_scr_data_size = platform_data.get_screen_blocks_cnt() << 3;
			update_desc();
			
			ShowDialog();
		}
		
		void event_ok(object sender, System.EventArgs e)
		{
			this.Close();

			m_filename		= get_exp_prefix() + Path.GetFileNameWithoutExtension( m_path_filename_ext );
			m_path			= Path.GetDirectoryName( m_path_filename_ext ) + Path.DirectorySeparatorChar;
			m_path_filename	= m_path + m_filename;

			m_level_prefix 	= get_exp_prefix() + CONST_FILENAME_LEVEL_PREFIX;
			
			StreamWriter sw = null;
			
			try
			{
				if( CheckBoxGenerateHFile.Checked )
				{
					m_C_writer = new StreamWriter( m_path + Path.GetFileNameWithoutExtension( m_path_filename_ext ) + ".h" );
					{
						utils.write_title( m_C_writer, true );
						
						m_C_writer.WriteLine( "\n#incasm( \"" + Path.GetFileName( m_path_filename_ext ) + "\" )\n" );
					}
				}
				
				sw = new StreamWriter( m_path_filename_ext );
				
				utils.write_title( sw );

				write_options( ( m_C_writer != null ) ? m_C_writer:sw );
				
				write_map_flags( CheckBoxGenerateHFile.Checked ? m_C_writer:sw );
				
				if( m_C_writer != null )
				{
					save_screen_struct();
				}
				
				if( CheckBoxExportEntities.Checked )
				{
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "\n#asm" );
					}
					
					m_data_mngr.export_entity_asm( ( ( m_C_writer != null ) ? m_C_writer:sw ), ".byte", "$" );
					
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "#endasm" );
					}
				}
				
				if( RBtnModeMultidirScroll.Checked )
				{
					save_multidir_scroll_mode( sw );
				}
				else
				{
					save_single_screen_mode( sw );
				}
			}
			catch( System.Exception _err )
			{
				MainForm.message_box( _err.Message, "Data Export Error", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error ); 
			}
			
			finally
			{
				if( sw != null )
				{
					sw.Dispose();
				}
				
				if( m_C_writer != null )
				{
					m_C_writer.Dispose();
					m_C_writer = null;
				}
			}
		}
		
		void save_screen_struct()
		{
			m_C_writer.WriteLine( "\n/* screen data */\n\ntypedef struct\n{" );
			m_C_writer.WriteLine( "#if\tFLAG_MODE_BIDIR_SCROLL + FLAG_MODE_STAT_SCR" );
			m_C_writer.WriteLine( "\tu8\tchr_id;" );
			m_C_writer.WriteLine( "#endif" );
			m_C_writer.WriteLine( "#if\tFLAG_MARKS" );
			m_C_writer.WriteLine( "\t//bits: 7-4 - bit mask of user defined adjacent screens ( Down(7)-Right(6)-Up(5)-Left(4) ); 3-0 - screen property" );
			m_C_writer.WriteLine( "\tu8\tmarks;" );
			m_C_writer.WriteLine( "#endif" );
			m_C_writer.WriteLine( "#if\tFLAG_MODE_STAT_SCR" );
			m_C_writer.WriteLine( "\tu16\tVDC_data_offset;" );
			m_C_writer.WriteLine( "#endif" );
			m_C_writer.WriteLine( "#if\tFLAG_MODE_BIDIR_SCROLL + FLAG_MODE_STAT_SCR" );
			m_C_writer.WriteLine( "\tu8\tscr_ind;" );
			m_C_writer.WriteLine( "#endif" );
			m_C_writer.WriteLine( "#if\tFLAG_LAYOUT_ADJ_SCR" );
			m_C_writer.WriteLine( "\t// adjacent screen pointers" );
			m_C_writer.WriteLine( "\tvoid*\tadj_scr[4];" );
			m_C_writer.WriteLine( "#endif" );
			m_C_writer.WriteLine( "#if\tFLAG_LAYOUT_ADJ_SCR_INDS" );
			m_C_writer.WriteLine( "\tu8\tadj_scr[4];" );
			m_C_writer.WriteLine( "#endif" );
			m_C_writer.WriteLine( "#if\tFLAG_ENTITIES" );
			m_C_writer.WriteLine( "\tu8\tents_cnt;" );
			m_C_writer.WriteLine( "\tmpd_ENTITY_INSTANCE*\tents[];" );
			m_C_writer.WriteLine( "#endif" );
			m_C_writer.WriteLine( "} mpd_SCREEN;" );
		}

		void write_options( StreamWriter _sw )
		{
			string comment = ( m_C_writer != null ) ? "//":";";
			
			_sw.WriteLine( comment + " export options:" );

			_sw.WriteLine( comment + "\t- tiles " + ( RBtnTiles4x4.Checked ? "4x4":"2x2" ) + ( CheckBoxRLE.Checked ? " (RLE)":"" ) + ( RBtnTilesDirColumns.Checked ? "/(columns)":"/(rows)" ) );
			
			_sw.WriteLine( comment + "\t- properties per " + ( RBtnPropPerBlock.Checked ? "block":"CHR" ) );
			
			if( RBtnModeMultidirScroll.Checked )
			{
				_sw.WriteLine( comment + "\t- mode: multidirectional scrolling" );
			}
			else
			if( RBtnModeBidirScroll.Checked )
			{
				_sw.WriteLine( comment + "\t- mode: bidirectional scrolling" );
			}
			else
			if( RBtnModeStaticScreen.Checked )
			{
				_sw.WriteLine( comment + "\t- mode: static screens" );
			}
			
			_sw.WriteLine( comment + "\t- layout: " + ( RBtnLayoutAdjacentScreens.Checked ? "adjacent screens":RBtnLayoutAdjacentScreenIndices.Checked ? "adjacent screen indices":"matrix" ) + ( CheckBoxExportMarks.Checked ? " (marks)":" (no marks)" ) );
			
			_sw.WriteLine( comment + "\t- " + ( CheckBoxExportEntities.Checked ? "entities " + ( RBtnEntityCoordScreen.Checked ? "(screen coordinates)":"(map coordinates)" ):"no entities" ) );
			
			_sw.WriteLine( "\n" );
		}
		
		void write_map_flags( StreamWriter _sw )
		{
			string c_def 		= CheckBoxGenerateHFile.Checked ? "#define ":"";
			string c_comment	= CheckBoxGenerateHFile.Checked ? "//":";";
			string c_hex_pref	= CheckBoxGenerateHFile.Checked ? "0x":"$";
			string c_def_eq		= CheckBoxGenerateHFile.Checked ? "":"= ";
			
			if( m_C_writer != null )
			{
				_sw.WriteLine( "/* exported data flags: */\n" );
				_sw.WriteLine( c_def + "FLAG_TILES2X2                 " + c_def_eq + ( RBtnTiles2x2.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_TILES4X4                 " + c_def_eq + ( RBtnTiles4x4.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_RLE                      " + c_def_eq + ( CheckBoxRLE.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_DIR_COLUMNS              " + c_def_eq + ( RBtnTilesDirColumns.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_DIR_ROWS                 " + c_def_eq + ( RBtnTilesDirRows.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_MODE_MULTIDIR_SCROLL     " + c_def_eq + ( RBtnModeMultidirScroll.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_MODE_BIDIR_SCROLL        " + c_def_eq + ( RBtnModeBidirScroll.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_MODE_BIDIR_STAT_SCR      0" );
				_sw.WriteLine( c_def + "FLAG_MODE_STAT_SCR            " + c_def_eq + ( RBtnModeStaticScreen.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_ENTITIES                 " + c_def_eq + ( CheckBoxExportEntities.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_ENTITY_SCR_COORDS        " + c_def_eq + ( CheckBoxExportEntities.Checked ? ( RBtnEntityCoordScreen.Checked ? "1":"0" ):"0" ) );
				_sw.WriteLine( c_def + "FLAG_ENTITY_MAP_COORS         " + c_def_eq + ( CheckBoxExportEntities.Checked ? ( RBtnEntityCoordMap.Checked ? "1":"0" ):"0" ) );
				_sw.WriteLine( c_def + "FLAG_LAYOUT_ADJ_SCR           " + c_def_eq + ( RBtnLayoutAdjacentScreens.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_LAYOUT_ADJ_SCR_INDS      " + c_def_eq + ( RBtnLayoutAdjacentScreenIndices.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_LAYOUT_MATRIX            " + c_def_eq + ( RBtnLayoutMatrix.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_MARKS                    " + c_def_eq + ( CheckBoxExportMarks.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_PROP_ID_PER_BLOCK        " + c_def_eq + ( RBtnPropPerBlock.Checked ? "1":"0" ) );
				_sw.WriteLine( c_def + "FLAG_PROP_ID_PER_CHR          " + c_def_eq + ( RBtnPropPerCHR.Checked ? "1":"0" ) );
			}
			else
			{
				_sw.WriteLine( c_def + "MAP_DATA_MAGIC " + c_def_eq + utils.hex( c_hex_pref, ( RBtnTiles2x2.Checked ? 1:2 ) |
				                                              		( CheckBoxRLE.Checked ? 4:0 ) |
				                                              		( RBtnTilesDirColumns.Checked ? 8:16 ) |
				                                              		( RBtnModeMultidirScroll.Checked ? 32:RBtnModeBidirScroll.Checked ? 64:128 ) | 
				                                              		( CheckBoxExportEntities.Checked ? 256:0 ) |
				                                              		( CheckBoxExportEntities.Checked ? ( RBtnEntityCoordScreen.Checked ? 512:1024 ):0 ) |
				                                              		( RBtnLayoutAdjacentScreens.Checked ? 2048:RBtnLayoutAdjacentScreenIndices.Checked ? 4096:8192 ) |
				                                              		( CheckBoxExportMarks.Checked ? 16384:0 ) |
				                                              		( RBtnPropPerBlock.Checked ? 32768:65536 ) ) );
				_sw.WriteLine( "\n" + c_comment + " data flags:" );
				_sw.WriteLine( c_def + "MAP_FLAG_TILES2X2                 " + c_def_eq + utils.hex( c_hex_pref, 1 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_TILES4X4                 " + c_def_eq + utils.hex( c_hex_pref, 2 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_RLE                      " + c_def_eq + utils.hex( c_hex_pref, 4 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_DIR_COLUMNS              " + c_def_eq + utils.hex( c_hex_pref, 8 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_DIR_ROWS                 " + c_def_eq + utils.hex( c_hex_pref, 16 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_MODE_MULTIDIR_SCROLL     " + c_def_eq + utils.hex( c_hex_pref, 32 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_MODE_BIDIR_SCROLL        " + c_def_eq + utils.hex( c_hex_pref, 64 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_MODE_STATIC_SCREENS      " + c_def_eq + utils.hex( c_hex_pref, 128 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_ENTITIES                 " + c_def_eq + utils.hex( c_hex_pref, 256 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_ENTITY_SCREEN_COORDS     " + c_def_eq + utils.hex( c_hex_pref, 512 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_ENTITY_MAP_COORS         " + c_def_eq + utils.hex( c_hex_pref, 1024 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_LAYOUT_ADJACENT_SCREENS  " + c_def_eq + utils.hex( c_hex_pref, 2048 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_LAYOUT_ADJACENT_SCR_INDS " + c_def_eq + utils.hex( c_hex_pref, 4096 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_LAYOUT_MATRIX            " + c_def_eq + utils.hex( c_hex_pref, 8192 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_MARKS                    " + c_def_eq + utils.hex( c_hex_pref, 16384 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_PROP_ID_PER_BLOCK        " + c_def_eq + utils.hex( c_hex_pref, 32768 ) );
				_sw.WriteLine( c_def + "MAP_FLAG_PROP_ID_PER_CHR          " + c_def_eq + utils.hex( c_hex_pref, 65536 ) );
			}
			
			_sw.WriteLine( "\n" + c_def + "MAPS_CNT\t" + c_def_eq + m_data_mngr.layouts_data_cnt );

			_sw.WriteLine( "\n" + c_def + "BAT_INDEX\t" + c_def_eq + m_BAT_index[ ComboBoxBAT.SelectedIndex ] );
			_sw.WriteLine( c_def + "CHRS_OFFSET\t" + c_def_eq + get_CHR_offset() + "\t" + c_comment + " first CHR index from the beginning of VRAM" );

			_sw.WriteLine( "\n" + c_def + "SCR_BLOCKS2x2_WIDTH\t" + c_def_eq + platform_data.get_screen_blocks_width() + "\t" + c_comment + " number of screen blocks (2x2) in width" );
			_sw.WriteLine( c_def + "SCR_BLOCKS2x2_HEIGHT\t" + c_def_eq + platform_data.get_screen_blocks_height() + "\t" + c_comment + " number of screen blocks (2x2) in height" );
			
			_sw.WriteLine( "\n" + c_def + "ScrTilesWidth\t" + c_def_eq + get_tiles_cnt_width( 1 ) + "\t" + c_comment + " number of screen tiles (" + ( RBtnTiles2x2.Checked ? "2x2":"4x4" ) + ") in width" );
			_sw.WriteLine( c_def + "ScrTilesHeight\t" + c_def_eq + get_tiles_cnt_height( 1 ) + "\t" + c_comment + " number of screen tiles (" + ( RBtnTiles2x2.Checked ? "2x2":"4x4" ) + ") in height" );

			_sw.WriteLine( "\n" + c_def + "ScrPixelsWidth\t" + c_def_eq + get_tiles_cnt_width( 1 ) * ( RBtnTiles2x2.Checked ? 16:32 ) + "\t" + c_comment + " screen width in pixels" );
			_sw.WriteLine( c_def + "ScrPixelsHeight\t" + c_def_eq + get_tiles_cnt_height( 1 ) * ( RBtnTiles2x2.Checked ? 16:32 ) + "\t" + c_comment + " screen height in pixels" );
			
			if( RBtnModeStaticScreen.Checked )
			{
				if( m_C_writer != null )
				{
					_sw.WriteLine( "\n" + c_def + "ScrGfxDataSize\t" + m_VDP_ready_scr_data_size + "\t// static screen data size in bytes\n" );
				}
				else
				{
					_sw.WriteLine( "\nScrGfxDataSize = " + m_VDP_ready_scr_data_size + "\t; static screen data size in bytes\n" );
				}
			}
		}
		
		private string get_C_data_prefix()
		{
			return ( m_C_writer != null ) ? get_exp_prefix() + "mpd_":m_filename + "_";
		}
		
		private void save_single_screen_mode( StreamWriter _sw )
		{
			BinaryWriter 	bw 			= null;
			BinaryWriter	bw_props	= null;
			
			StreamWriter	uni_stream	= ( m_C_writer != null ) ? m_C_writer:_sw;
			
			layout_data level_data = null;

			List< tiles_data > scr_tiles_data = m_data_mngr.get_tiles_data();
			
			int n_banks				= scr_tiles_data.Count;
			int n_levels 			= m_data_mngr.layouts_data_cnt;
			int n_scr_X 			= 0;
			int n_scr_Y				= 0;
			int n_screens 			= 0;
			int scr_ind				= 0;
			int scr_ind_opt			= 0;
			int bank_ind			= 0;
			int scr_key				= 0;
			int tile_n				= 0;
			int tiles_cnt			= 0;
			int tile_offs_x			= 0;
			int tile_offs_y			= 0;
			int block_n				= 0;
			int bank_n				= 0;
			int blocks_props_size	= 0;
			int blocks_size			= 0;
			int data_offset 		= 0;
			int common_scr_ind		= 0;
			int max_scr_tile		= 0;
			int max_tile_ind		= 0;
			int max_scr_block		= 0;
			int max_block_ind		= 0;
			int start_scr_ind		= 0;
			int ents_cnt;
			
			int scr_width_blocks_mul2 	= 0;
			int scr_height_blocks_mul2 	= 0;
			int scr_height_blocks_mul4 	= 0;
			
			int scr_width_blocks 	= platform_data.get_screen_blocks_width();
			int scr_height_blocks 	= platform_data.get_screen_blocks_height();

			uint block_data		= 0;
			
			ushort tile_id			= 0;
			ushort block_id			= 0;
			
			screen_data tile_inds		= null;
			ushort[] tile_attrs_arr		= new ushort[ 16 ];
			ushort[] block_attrs_arr	= new ushort[ 8 ];
			
			long data_size 			= 0;
			long exp_data_size		= 0;

			bool valid_bank;
			bool enable_comments;
			
			string label 			= null;
			string level_prefix_str	= null;
			string label_props		= null;
			string scr_arr			= null;
			string data_offset_str	= null;
			string maps_arr			= null;
			string maps_scr_arr		= null;
			
			string c_comment		= CheckBoxGenerateHFile.Checked ? "//":";";
			
			string c_data_prefix		= get_C_data_prefix();
			string c_data_prefix_no_exp = ( m_C_writer != null ) ? skip_exp_pref( c_data_prefix ):c_data_prefix;
			
			exp_scr_data		exp_scr;
			layout_screen_data	scr_data;
			tiles_data 			tiles = null;
			
			List< tiles_data > 	banks 			= new List< tiles_data >( 10 );			
			List< int >			max_tile_inds	= new List< int >( 10 );
			List< int >			max_block_inds	= new List< int >( 10 );
			
			scr_width_blocks_mul2	= scr_width_blocks << 1;
			scr_height_blocks_mul2	= scr_height_blocks << 1;
			scr_height_blocks_mul4	= scr_height_blocks << 2;

			ushort[] attrs_chr = new ushort[ ( scr_width_blocks * scr_height_blocks ) << 2 ];
						
			exp_scr_data._tiles_offset  = 0;
			exp_scr_data._blocks_offset = 0;

			Dictionary< int, exp_scr_data > screens = new Dictionary< int, exp_scr_data >( 100 );

			scr_ind = 0;		// global screen index
			scr_ind_opt = 0;	// optimized screen index
			
			// collect all banks & screens data in a project
			for( bank_n = 0; bank_n < m_data_mngr.tiles_data_cnt; bank_n++ )
			{
				tiles = m_data_mngr.get_tiles_data( bank_n );
				
				valid_bank = false;
				
				max_tile_ind = max_block_ind = Int32.MinValue;
				
				for( int scr_n = 0; scr_n < tiles.screen_data_cnt(); scr_n++ )
				{
					if( check_screen_layouts( scr_ind ) == true )
					{
						if( scr_ind_opt > utils.CONST_SCREEN_MAX_CNT - 1 )
						{
							throw new Exception( "The screen index is out of range!\nThe maximum number of screens allowed to export: " + utils.CONST_SCREEN_MAX_CNT );
						}
						
						valid_bank = true;
						
						exp_scr = new exp_scr_data( scr_ind_opt++, tiles );
						
						screens[ ( bank_n << 8 ) | scr_n ] = exp_scr;
						
						if( m_data_mngr.screen_data_type == data_sets_manager.EScreenDataType.sdt_Tiles4x4 ) 
						{
							for( tile_n = 0; tile_n < platform_data.get_screen_tiles_cnt(); tile_n++ )
							{
								tile_offs_x = ( tile_n % platform_data.get_screen_tiles_width() );
								tile_offs_y = ( tile_n / platform_data.get_screen_tiles_width() );
								
								tile_id = exp_scr.m_tiles.get_screen_tile( scr_n, tile_n );
		
								exp_scr.m_scr_tiles.set_tile( tile_offs_x * platform_data.get_screen_tiles_height() + tile_offs_y, tile_id );
								
								if( RBtnTiles2x2.Checked )
								{
									tile_offs_x <<= 1;
									tile_offs_y <<= 1;
									
									// fill the array of all tiles 2x2 in a level
									for( block_n = 0; block_n < utils.CONST_TILE_SIZE; block_n++ )
									{
										block_id = tiles.get_tile_block( tile_id, block_n );
										
										if( ( tile_offs_x + ( block_n & 0x01 ) ) < platform_data.get_screen_blocks_width() && ( tile_offs_y + ( ( block_n & 0x02 ) >> 1 ) ) < platform_data.get_screen_blocks_height() )
										{
											exp_scr.m_scr_blocks.set_tile( ( tile_offs_x * scr_height_blocks ) + ( ( block_n & 0x01 ) == 0x01 ? scr_height_blocks:0 ) + tile_offs_y + ( ( block_n & 0x02 ) == 0x02 ? 1:0 ), block_id );
										}
									}
								}
							}
						}
						else
						{
							for( block_n = 0; block_n < platform_data.get_screen_blocks_cnt(); block_n++ )
							{
								tile_offs_x = ( block_n % platform_data.get_screen_blocks_width() );
								tile_offs_y = ( block_n / platform_data.get_screen_blocks_width() );
								
								exp_scr.m_scr_blocks.set_tile( ( tile_offs_x * scr_height_blocks ) + tile_offs_y, tiles.get_screen_tile( scr_n, block_n ) );
							}
						}
						
						max_scr_tile = exp_scr.m_scr_tiles.max_val();
						
						if( max_tile_ind < max_scr_tile )
						{
							max_tile_ind = max_scr_tile;
						}
						
						if( RBtnTiles2x2.Checked )
						{
							max_scr_block = exp_scr.m_scr_blocks.max_val();
							
							if( max_block_ind < max_scr_block )
							{
								max_block_ind = max_scr_block;
							}
						}
					}
					
					++scr_ind;
				}
				
				if( valid_bank == true )
				{
					banks.Add( tiles );
					
					max_tile_inds.Add( 1 + max_tile_ind );
					
					if( RBtnTiles2x2.Checked )
					{
						max_block_inds.Add( ( 1 + max_block_ind ) << 3 );
					}
					else
					{
						max_block_ind = tiles.get_first_free_block_id();
						max_block_ind = max_block_ind < 0 ? platform_data.get_max_blocks_cnt():max_block_ind;
						
						max_block_inds.Add( max_block_ind << 3 );
					}
				}
			}
			
			var screen_sorted_keys_list = screens.Keys.ToList();
			screen_sorted_keys_list.Sort();

			// global data writing
			{
				_sw.WriteLine( "\n; *** GLOBAL DATA ***\n" );

				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "\n" );
				}
				
				string chr_arr	= null;
				string chr_size	= null;
				
				// CHR
				for( bank_n = 0; bank_n < banks.Count; bank_n++ )
				{
					tiles = banks[ bank_n ];
					
					// write CHR bank data
					label = get_exp_prefix() + "chr" + bank_n;
					bw = new BinaryWriter( File.Open( m_path_filename + "_" + label + CONST_BIN_EXT, FileMode.Create ) );
					{
						data_size = tiles.export_CHR( bw );
					}
					bw.Close();
					
					_sw.WriteLine( ( ( m_C_writer != null ) ? "":";" ) + label + ":\t.incbin \"" + m_filename + "_" + label + CONST_BIN_EXT + "\"\t\t; (" + data_size + ")" );
					
					if( m_C_writer != null )
					{
						chr_arr	 += "\t.word bank(" + label + ")\n";
					}
					
					chr_arr	 += "\t.word " + label + "\n";
					chr_size += "\t.word " + data_size + "\t; (" + label + ")\n";
					
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "extern u16*\t" + skip_exp_pref( label ) + ";" );
					}
					
					exp_data_size += data_size;
				}

				_sw.WriteLine( "\n" );

				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "\n#asm\n" + c_data_prefix + "CHRs:\n" + chr_arr + "#endasm\n\nextern u16\t" + c_data_prefix_no_exp + "CHRs[];" );
					m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "CHRs_size;" );
				}
				else
				{
					_sw.WriteLine( c_data_prefix + "CHRs:\n" + chr_arr );
				}
				
				_sw.WriteLine( c_data_prefix + "CHRs_size:\n" + chr_size );
				
				// static screens ( VDC-READY DATA ):
				// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
				// [SKIP] attrs map arr - attributes array for each screen
				// [SKIP] blocks arr	- blocks data array
				// map arr				- GFX array for each screen (RLE) (VDC-ready) [CHRs]
				// plts arr 			- bank palettes
				// props arr 			- bank props
				//
				// 2x2:
				// ~~~~
				//   map arr (props)	- props indices array for each screen
				//
				// 4x4:
				// ~~~~
				//   [SKIP] attrs		- array of attributes
				//   map arr (tiles)	- tiles 4x4 indices array for each screen
				//   tiles arr (props)	- tiles array with props(blocks) indices 
				
				// blocks&props
				{
					label_props = "Props";
					bw_props = new BinaryWriter( File.Open( m_path_filename + "_" + label_props + CONST_BIN_EXT, FileMode.Create ) );
				
					data_offset = 0;
					data_offset_str = "";
					
					for( bank_n = 0; bank_n < banks.Count; bank_n++ )
					{
						tiles = banks[ bank_n ];

						if( m_data_mngr.screen_data_type == data_sets_manager.EScreenDataType.sdt_Tiles4x4 )
						{
							blocks_props_size = ( 1 + utils.get_ulong_arr_max_val( tiles.tiles, max_tile_inds[ bank_n ] ) ) << 2;
						}
						else
						{
							blocks_props_size = tiles.get_first_free_block_id();
							blocks_props_size = ( ( blocks_props_size < 0 ) ? platform_data.get_max_blocks_cnt():blocks_props_size ) << 2;
						}
						
						for( block_n = 0; block_n < blocks_props_size; block_n++ )
						{
							block_data = tiles.blocks[ block_n ];
							
							if( RBtnPropPerBlock.Checked && ( block_n % 4 ) != 0 )
							{
								continue;
							}
							
							bw_props.Write( ( byte )tiles_data.get_block_flags_obj_id( block_data ) );
						}
						
						data_offset_str += "\t.word " + ( data_offset / ( RBtnPropPerBlock.Checked ? 4:1 ) ) + "\t; (chr" + bank_n + ")\n";

						data_offset += blocks_props_size;
					}

					data_size = bw_props.BaseStream.Length;
					align_word( bw_props );
					bw_props.Close();
					
					exp_data_size += data_size;
					
					_sw.WriteLine( c_data_prefix + label_props + ":\t.incbin \"" + m_filename + "_" + label_props + CONST_BIN_EXT + "\"\t; (" + data_size + ") block properties array of all exported data banks ( " + ( RBtnPropPerCHR.Checked ? "4 bytes":"1 byte" ) + " per block )\n" );
					
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "extern u8*\t" + c_data_prefix_no_exp + label_props + ";" );
					}
					
					_sw.WriteLine( c_data_prefix + "PropsOffs:" );
					
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "PropsOffs;" );
						
						data_offset_str += "\t.word " + ( data_offset / ( RBtnPropPerBlock.Checked ? 4:1 ) ) + "\t; data end\n";
					}
					
					_sw.WriteLine( data_offset_str );
				}
				
				// tiles
				{
					if( RBtnTiles4x4.Checked )
					{
						// write tiles data
						label = "Tiles";
					
						bw = new BinaryWriter( File.Open( m_path_filename + "_" + label + CONST_BIN_EXT, FileMode.Create ) );
					
						data_offset = 0;
						data_offset_str = "";
						
						// tiles
						for( bank_n = 0; bank_n < banks.Count; bank_n++ )
						{
							max_tile_ind = max_tile_inds[ bank_n ];	// one based index

							tiles = banks[ bank_n ];
							
							for( int i = 0; i < max_tile_ind; i++ )
							{
								bw.Write( tiles_data.tile_convert_ulong_to_uint_reverse( tiles.tiles[ i ] ) );
							}
							
							data_offset_str += "\t.word " + data_offset + "\t; (chr" + bank_n + ")\n";
							
							data_offset += max_tile_inds[ bank_n ] << 2;
						}
						
						data_size = bw.BaseStream.Length;
						bw.Close();
					
						exp_data_size += data_size;
						
						_sw.WriteLine( c_data_prefix + label + ":\t.incbin \"" + m_filename + "_" + label + CONST_BIN_EXT + "\"\t; (" + data_size + ") 4x4 tiles array of all exported data banks ( 4 bytes per tile )\n" );
						
						if( m_C_writer != null )
						{
							m_C_writer.WriteLine( "extern u8*\t" + c_data_prefix_no_exp + label + ";" );
						}
						
						_sw.WriteLine( c_data_prefix + "TilesOffs:" );
	
						_sw.WriteLine( data_offset_str );
						
						if( m_C_writer != null )
						{
							m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "TilesOffs;" );
						}
					}
					
					// save blocks offsets by CHR bank
					{
						data_offset = 0;
						data_offset_str = "";
						
						// tiles
						for( bank_n = 0; bank_n < banks.Count; bank_n++ )
						{
							tiles = banks[ bank_n ];
							
							data_offset_str += "\t.word " + data_offset + "\t; (chr" + bank_n + ")\n";
							
							if( m_data_mngr.screen_data_type == data_sets_manager.EScreenDataType.sdt_Tiles4x4 )
							{
								data_offset += ( 1 + utils.get_ulong_arr_max_val( tiles.tiles, max_tile_inds[ bank_n ] ) ) << 3;
							}
							else
							{
								blocks_size = tiles.get_first_free_block_id();
								data_offset += ( ( blocks_size < 0 ) ? platform_data.get_max_blocks_cnt():blocks_size ) << 3;
							}
						}
						
						_sw.WriteLine( c_data_prefix + "BlocksOffs:" );
	
						_sw.WriteLine( data_offset_str );
						
						if( m_C_writer != null )
						{
							m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "BlocksOffs;" );
						}
					}
				}
				
				// save VDC-ready data for STATIC SCREENS mode
				if( RBtnModeStaticScreen.Checked )
				{
					label = "VDCScr";
					bw = new BinaryWriter( File.Open( m_path_filename + "_" + label + CONST_BIN_EXT, FileMode.Create ) );

					data_offset = 0;
					
					foreach( var key in screen_sorted_keys_list )
					{
						exp_scr = screens[ key ];
						
						tile_inds	= exp_scr.m_scr_tiles;
						tiles_cnt 	= tile_inds.length;
						tiles 		= exp_scr.m_tiles;

#if DEF_DBG_PPU_READY_DATA_SAVE_IMG
						Bitmap 	tile_bmp	= null;
						Bitmap 	scr_bmp 	= new Bitmap( platform_data.get_screen_width_pixels(), utils.CONST_SCREEN_HEIGHT_PIXELS );
	
						Graphics scr_gfx 			= Graphics.FromImage( scr_bmp );
						scr_gfx.InterpolationMode 	= InterpolationMode.NearestNeighbor;
						scr_gfx.PixelOffsetMode 	= PixelOffsetMode.HighQuality;
#endif
						// save CHR ids
						fill_screen_attrs_per_CHR( 	attrs_chr,
						                          	tiles, 
						                          	tile_inds, 
						                          	true,//force_swapping 
						                          	scr_width_blocks_mul2, 
						                          	scr_height_blocks_mul2, 
						                          	scr_height_blocks_mul4,
													exp_scr.m_scr_blocks );
#if DEF_DBG_PPU_READY_DATA_SAVE_IMG
						for( int i = 0; i < attrs_chr.Length; i++ )
						{
							int plt_ind	= 0;
							tiles.from_CHR_bank_to_spr8x8( CHRs_arr[ i ], utils.tmp_spr8x8_buff );
							
							tile_bmp = utils.create_bitmap( utils.tmp_spr8x8_buff, 8, 8, 0, false, plt_ind, palette_group.Instance.get_palettes_arr() );
							
							scr_gfx.DrawImage( tile_bmp, ( i % 32 ) << 3, ( i / 32 ) << 3 );
						}
						scr_bmp.Save( m_path_filename + "_Scr" + exp_scr.m_scr_ind + ".png" );
						scr_bmp.Dispose();
						tile_bmp.Dispose();
#endif
						exp_scr.m_VDC_scr_offset = data_offset;
						screens[ key ] = exp_scr;
						
						if( compress_and_save_ushort( bw, attrs_chr, ref data_offset ) == false )
						{
							_sw.Close();
							bw.Close();
							throw new System.Exception( "Can't compress an empty data!" );
						}
					}
					
					data_size = bw.BaseStream.Length;
					bw.Close();
					
					exp_data_size += data_size;
					
					_sw.WriteLine( c_data_prefix + label + ":\t.incbin \"" + m_filename + "_" + label + CONST_BIN_EXT + "\"\t; " + ( CheckBoxRLE.Checked ? "compressed ":"" ) + "(" + data_offset + ( CheckBoxRLE.Checked ? " / " + ( screens.Count * m_VDP_ready_scr_data_size ):"" ) + ") VDC-ready data array for each screen (" + m_VDP_ready_scr_data_size + " bytes per screen)" );
					
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + label + ";\t// " + ( CheckBoxRLE.Checked ? "compressed ":"" ) + "VDC-ready data array for each screen" );
					}
				}
				
				// attributes array of 4x4 tiles ONLY and for the BIDIRECTIONAL SCROLLING mode !!!
				if( RBtnModeBidirScroll.Checked )
				{
					label = "Attrs";
					bw = new BinaryWriter( File.Open( m_path_filename + "_" + label + CONST_BIN_EXT, FileMode.Create ) );
					
					for( bank_n = 0; bank_n < banks.Count; bank_n++ )
					{
						tiles = banks[ bank_n ];
						
						if( m_data_mngr.screen_data_type == data_sets_manager.EScreenDataType.sdt_Tiles4x4 )
						{
							blocks_props_size = ( 1 + utils.get_ulong_arr_max_val( tiles.tiles, max_tile_inds[ bank_n ] ) ) << 2;
						}
						else
						{
							blocks_props_size = tiles.get_first_free_block_id();
							blocks_props_size = ( ( blocks_props_size < 0 ) ? platform_data.get_max_blocks_cnt():blocks_props_size ) << 2;
						}
							
						for( block_n = 0; block_n < blocks_props_size; block_n++ )
						{
							block_data = tiles.blocks[ block_n ];
							
							bw.Write( get_screen_attribute( block_data ) );
						}
					}
					
					data_size = bw.BaseStream.Length;
					bw.Close();
					
					exp_data_size += data_size;
					
					_sw.WriteLine( c_data_prefix + label + ":\t.incbin \"" + m_filename + "_" + label + CONST_BIN_EXT + "\"\t; (" + data_size + ") 2x2 tiles attributes array of all exported data banks ( 2 bytes per attribute )" );
					
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + label + ";" );
					}
				}
				
				// map
				{
					// tiles indices array for each screen
					label = "TilesScr";
					bw = new BinaryWriter( File.Open( m_path_filename + "_" + label + CONST_BIN_EXT, FileMode.Create ) );

					foreach( var key in screen_sorted_keys_list ) 
					{ 
						if( RBtnTiles4x4.Checked )
						{
							tile_inds = screens[ key ].m_scr_tiles;
						}
						else
						{
							tile_inds = screens[ key ].m_scr_blocks;
						}
						
						if( RBtnTilesDirRows.Checked )
						{
							tile_inds.swap_col_row_data();
						}						
						
						for( int i = 0; i < tile_inds.length; i++ )
						{
							bw.Write( ( byte )tile_inds.get_tile( i ) );
						}
					}
					
					data_size = bw.BaseStream.Length;
					bw.Close();
					
					exp_data_size += data_size;
					
					_sw.WriteLine( c_data_prefix + label + ":\t.incbin \"" + m_filename + "_" + label + CONST_BIN_EXT + "\"\t; (" + data_size + ") " + ( RBtnTiles2x2.Checked ? "2x2":"4x4" ) + " tiles array for each screen ( " + ( RBtnTiles2x2.Checked ? ( scr_width_blocks * scr_height_blocks ):platform_data.get_screen_tiles_cnt() ) + " bytes per screen \\ 1 byte per tile )" );
					
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "extern u8*\t" + c_data_prefix_no_exp + label + ";" );
					}
				}
				
				// palettes
				{
					label = "Plts";
					bw = new BinaryWriter( File.Open( m_path_filename + "_" + label + CONST_BIN_EXT, FileMode.Create ) );
					
					for( bank_n = 0; bank_n < banks.Count; bank_n++ )
					{
						tiles = banks[ bank_n ];
						
						for( int i = 0; i < tiles.palettes_arr.Count; i++ )
						{
							utils.write_int_as_ushort_arr( bw, tiles.palettes_arr[ i ].m_palette0 );
							utils.write_int_as_ushort_arr( bw, tiles.palettes_arr[ i ].m_palette1 );
							utils.write_int_as_ushort_arr( bw, tiles.palettes_arr[ i ].m_palette2 );
							utils.write_int_as_ushort_arr( bw, tiles.palettes_arr[ i ].m_palette3 );
						}
					}
					
					data_size = bw.BaseStream.Length;
					bw.Close();
					
					exp_data_size += data_size;
					
					_sw.WriteLine( c_data_prefix + label + ":\t.incbin \"" + m_filename + "_" + label + CONST_BIN_EXT + "\"\t; (" + data_size + ") palettes array of all exported data banks ( data offset = chr_id * " + ( ( platform_data.get_fixed_palette16_cnt() << 4 ) << 1 ) + " )" );
					
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + label + ";" );
					}
				}
			}
			
			maps_arr = c_data_prefix + "MapsArr:\n";

			if( RBtnLayoutAdjacentScreenIndices.Checked )
			{
				maps_scr_arr = c_data_prefix + "MapsScrArr:\n";
			}

			for( int level_n = 0; level_n < n_levels; level_n++ )
			{
				enable_comments = true;
				
				level_data = m_data_mngr.get_layout_data( level_n );
				
				level_prefix_str = m_level_prefix + level_n;
				
				maps_arr += "\t.word " + level_prefix_str + "_StartScr\n";
				
				check_ent_instances_cnt( level_data, level_prefix_str );

				uni_stream.WriteLine( "\n" + c_comment + " *** " + level_prefix_str + " ***\n" );
				
				n_scr_X = level_data.get_width();
				n_scr_Y = level_data.get_height();

				if( RBtnLayoutAdjacentScreenIndices.Checked )
				{
					scr_arr			= level_prefix_str + "_ScrArr";
					maps_scr_arr	+= "\t.word " + scr_arr + "\n";

					scr_arr += ":";
					
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "extern mpd_SCREEN*\t" + skip_exp_pref( level_prefix_str ) + "_ScrArr[];" );
					}
				}
				
				ents_cnt = 0;
				
				for( int scr_n_Y = 0; scr_n_Y < n_scr_Y; scr_n_Y++ )
				{
					for( int scr_n_X = 0; scr_n_X < n_scr_X; scr_n_X++ )
					{
						scr_data = level_data.get_data( scr_n_X, scr_n_Y );
						
						if( scr_data.m_scr_ind != layout_data.CONST_EMPTY_CELL_ID )
						{
							// determine which bank the screen belongs to
							n_screens = 0;
							for( bank_ind = 0; bank_ind < n_banks; bank_ind++ )
							{
								n_screens += scr_tiles_data[ bank_ind ].screen_data_cnt();
								
								if( scr_data.m_scr_ind < n_screens )
								{
									break;
								}
							}
							
							// convert a screen index into local index in the bank
							scr_ind = scr_data.m_scr_ind - ( n_screens - scr_tiles_data[ bank_ind ].screen_data_cnt() );
							
							scr_key = ( bank_ind << 8 ) | scr_ind;
							
							if( screens.ContainsKey( scr_key ) )
							{
								common_scr_ind = scr_n_Y * n_scr_X + scr_n_X;
								
								if( enable_comments )
								{
									start_scr_ind = level_data.get_start_screen_ind();
									
									if( start_scr_ind < 0 )
									{
										MainForm.message_box( "The start screen wasn't assigned to layout: " + level_n + "\n\nWARNING: A first valid screen will be used as a start one.", "Start Screen Warning", MessageBoxButtons.OK );
									}
									
									start_scr_ind = ( start_scr_ind < 0 ) ? common_scr_ind:start_scr_ind;

									if( RBtnLayoutMatrix.Checked )
									{
										if( m_C_writer != null )
										{
											m_C_writer.WriteLine( "extern mpd_SCREEN*\t" + skip_exp_pref( level_prefix_str ) + "_Layout[];\n" );
											m_C_writer.WriteLine( "extern u8\t" + skip_exp_pref( level_prefix_str ) + "_WScrCnt = " + level_data.get_width() + ";\t// number of screens in width" );
											m_C_writer.WriteLine( "extern u8\t" + skip_exp_pref( level_prefix_str ) + "_HScrCnt = " + level_data.get_height() + ";\t// number of screens in height" );
										}
										
										if( m_C_writer != null )
										{
											m_C_writer.WriteLine( "extern u8\t" + skip_exp_pref( level_prefix_str ) + "_StartScr;" );
											m_C_writer.WriteLine( "\n#asm" );
										}

										uni_stream.WriteLine( level_prefix_str + "_StartScr:\t.byte " + start_scr_ind + "\n" );
										
										level_data.export_asm( uni_stream, level_prefix_str, null, ".byte", ".word", ".word", "$", false, false, false, false, ( m_C_writer != null ? false:true ) );
									}
									else
									{
										if( m_C_writer != null )
										{
											m_C_writer.WriteLine( "extern mpd_SCREEN*\t" + skip_exp_pref( level_prefix_str ) + "_StartScr;" );
											m_C_writer.WriteLine( "\n#asm" );
										}
										
										uni_stream.WriteLine( level_prefix_str + "_StartScr:\t.word " + level_prefix_str + "Scr" + start_scr_ind + "\n" );
									}
								}
								
								exp_scr = screens[ scr_key ];
								
								uni_stream.WriteLine( level_prefix_str + "Scr" + common_scr_ind + ":" );
								uni_stream.WriteLine( "\t.byte " + banks.IndexOf( exp_scr.m_tiles ) + ( enable_comments ? "\t; chr_id":"" ) );
								
								if( CheckBoxExportMarks.Checked )
								{
									uni_stream.WriteLine( "\t.byte " + utils.hex( "$", scr_data.mark_adj_scr_mask ) + ( enable_comments ? "\t; (marks) bits: 7-4 - bit mask of user defined adjacent screens ( Down(7)-Right(6)-Up(5)-Left(4) ); 3-0 - screen property":"" ) );
								}
								
								if( RBtnModeStaticScreen.Checked )
								{
									uni_stream.WriteLine( "\n\t.word " + exp_scr.m_VDC_scr_offset + ( enable_comments ? "\t; " + m_filename + "_VDCScr offset":"" ) );
								}
								
								uni_stream.WriteLine( "\n\t.byte " + exp_scr.m_scr_ind + ( enable_comments ? "\t; screen index":"" ) );
								
								uni_stream.WriteLine( "" );
								
								if( RBtnLayoutAdjacentScreens.Checked )
								{
									uni_stream.WriteLine( "\t.word " + get_adjacent_screen_label( level_n, level_data, common_scr_ind, -1,	 0	) + ( enable_comments ? "\t; left adjacent screen":"" ) );
									uni_stream.WriteLine( "\t.word " + get_adjacent_screen_label( level_n, level_data, common_scr_ind,  0,	-1	) + ( enable_comments ? "\t; up adjacent screen":"" ) );
									uni_stream.WriteLine( "\t.word " + get_adjacent_screen_label( level_n, level_data, common_scr_ind,  1,	 0	) + ( enable_comments ? "\t; right adjacent screen":"" ) );
									uni_stream.WriteLine( "\t.word " + get_adjacent_screen_label( level_n, level_data, common_scr_ind,  0,   1	) + ( enable_comments ? "\t; down adjacent screen\n":"\n" ) );
								}
								else
								if( RBtnLayoutAdjacentScreenIndices.Checked )
								{
									if( enable_comments )
									{
										uni_stream.WriteLine( "; adjacent screen indices ( the valid values are $00 - $FE, $FF - means no screen )" );
										uni_stream.WriteLine( "; use the " + m_level_prefix + level_n + "_ScrArr array to get a screen description by adjacent screen index" );
									}
									
									uni_stream.WriteLine( "\t.byte " + get_adjacent_screen_index( level_n, level_data, common_scr_ind, -1,	 0	) + ( enable_comments ? "\t; left adjacent screen index":"" ) );
									uni_stream.WriteLine( "\t.byte " + get_adjacent_screen_index( level_n, level_data, common_scr_ind,  0,	-1	) + ( enable_comments ? "\t; up adjacent screen index":"" ) );
									uni_stream.WriteLine( "\t.byte " + get_adjacent_screen_index( level_n, level_data, common_scr_ind,  1,	 0	) + ( enable_comments ? "\t; right adjacent screen index":"" ) );
									uni_stream.WriteLine( "\t.byte " + get_adjacent_screen_index( level_n, level_data, common_scr_ind,  0,	 1	) + ( enable_comments ? "\t; down adjacent screen index\n":"\n" ) );
									
									scr_arr += "\n\t.word " + m_level_prefix + level_n + "Scr" + common_scr_ind;
								}
								
								if( CheckBoxExportEntities.Checked )
								{
									scr_data.export_entities_asm( uni_stream, ref ents_cnt, level_prefix_str + "Scr" + common_scr_ind + "EntsArr", ".byte", ".word", ".word", "$", RBtnEntityCoordScreen.Checked, scr_n_X, scr_n_Y, enable_comments );
									
									_sw.WriteLine( "" );
								}
								
								enable_comments = false;
							}
							else
							{
								throw new Exception( "Unexpected error! Can't find a screen data!" );
							}
						}
					}
				}
				
				if( RBtnLayoutAdjacentScreenIndices.Checked )
				{
					uni_stream.WriteLine( "; screens array" );
					uni_stream.WriteLine( scr_arr + "\n" );
				}
				
				if( CheckBoxExportEntities.Checked )
				{
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "#endasm" );
						m_C_writer.WriteLine( "\nconst u8\t" + CONST_FILENAME_LEVEL_PREFIX + level_n + "_EntInstCnt = " + level_data.get_ent_instances_cnt() + ";\t// number of entities instances\n" );
					}
					else
					{
						_sw.WriteLine( CONST_FILENAME_LEVEL_PREFIX + level_n + "_EntInstCnt\t = " + level_data.get_ent_instances_cnt() + "\t; number of entities instances\n" );
					}
				}
				else
				{
					if( m_C_writer != null )
					{
						m_C_writer.WriteLine( "#endasm" );
					}
				}
			}

			// save MapsArr
			{
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "\nextern mpd_SCREEN** " + c_data_prefix_no_exp + "MapsArr[];" );
					m_C_writer.WriteLine( "\n#asm" );
				}
				
				uni_stream.WriteLine( maps_arr );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "#endasm" );
				}
			}
			
			// save MapsScrArr
			if( RBtnLayoutAdjacentScreenIndices.Checked )
			{
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "\nextern mpd_SCREEN** " + c_data_prefix_no_exp + "MapsScrArr[];" );
					m_C_writer.WriteLine( "\n#asm" );
				}
				
				uni_stream.WriteLine( maps_scr_arr );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "#endasm" );
				}
			}
			
			foreach( var key in screens.Keys ) { screens[ key ].destroy(); }
			
			if( exp_data_size > 8192 )
			{
				MainForm.message_box( "The exported binary data size exceeds 8K ( " + exp_data_size + " B ) !", "Data Export Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning );
			}
		}

		private void align_word( BinaryWriter _br )
		{
			if( ( _br.BaseStream.Length & 0x01 ) != 0 )
			{
				_br.Write( ( byte )0 );
			}
		}
	
		private bool compress_and_save_byte( BinaryWriter _bw, byte[] _data, ref int _data_offset )
		{
			if( CheckBoxRLE.Checked )
			{
				byte[] rle_data_arr	= null;

				int rle_data_size = utils.RLE8( _data, ref rle_data_arr );
				
				if( rle_data_size < 0 )
				{
					return false;
				}
				else
				{
					_bw.Write( rle_data_arr, 0, rle_data_size );
					
					_data_offset += rle_data_size;
				}
				
				rle_data_arr = null;
			}
			else
			{
				_bw.Write( _data );
				
				_data_offset += _data.Length;
			}
			
			return true;
		}

		private bool compress_and_save_ushort( BinaryWriter _bw, ushort[] _data, ref int _data_offset )
		{
			if( CheckBoxRLE.Checked )
			{
				ushort[] rle_data_arr	= null;

				int rle_data_size = utils.RLE16( _data, ref rle_data_arr );
				
				if( rle_data_size < 0 )
				{
					return false;
				}
				else
				{
					utils.write_ushort_arr( _bw, rle_data_arr, rle_data_size );
					
					_data_offset += rle_data_size * sizeof( ushort );
				}
				
				rle_data_arr = null;
			}
			else
			{
				utils.write_ushort_arr( _bw, _data, _data.Length );
				
				_data_offset += _data.Length * sizeof( ushort );
			}
			
			return true;
		}
		
		private bool compress_and_save_byte( BinaryWriter _bw, byte[] _data )
		{
			int offset = 0;
			
			return compress_and_save_byte( _bw, _data, ref offset );
		}

		private bool compress_and_save_ushort( BinaryWriter _bw, ushort[] _data )
		{
			int offset = 0;
			
			return compress_and_save_ushort( _bw, _data, ref offset );
		}
		
		private void fill_screen_attrs_per_CHR( ushort[] 		_attrs_chr,
		                                       	tiles_data 		_tiles, 
		                                       	screen_data 		_tile_inds, 
		                                       	bool 			_force_swapping, 
		                                       	int 			_scr_width_blocks_mul2, 
		                                       	int 			_scr_height_blocks_mul2, 
		                                       	int 			_scr_height_blocks_mul4,
		                                       	screen_data		_block_inds )
		{
			ushort tile_id			= 0;
			
			int tile_x				= 0;
			int tile_y				= 0;
			int block_x				= 0;
			int block_y				= 0;
			int chr_x				= 0;
			int chr_y				= 0;
			int tile_offs_x			= 0;
			int tile_offs_y			= 0;
			
			int tile_n;
			int block_n;
			int chr_n;
			int tiles_cnt = _tile_inds.length;
			
			if( m_data_mngr.screen_data_type == data_sets_manager.EScreenDataType.sdt_Tiles4x4 )
			{
				for( tile_n = 0; tile_n < tiles_cnt; tile_n++ )
				{
					tile_id = _tile_inds.get_tile( tile_n );
	
					tile_offs_x = ( tile_n / platform_data.get_screen_tiles_height() ) << 1;
					tile_offs_y = ( tile_n % platform_data.get_screen_tiles_height() ) << 1;
	
					tile_x	= ( ( tile_offs_x << 1 ) * _scr_height_blocks_mul2 );
					tile_y	= ( tile_offs_y << 1 );
					
					for( block_n = 0; block_n < utils.CONST_TILE_SIZE; block_n++ )
					{
						if( ( tile_offs_x + ( block_n & 0x01 ) ) < platform_data.get_screen_blocks_width() && ( tile_offs_y + ( ( block_n & 0x02 ) >> 1 ) ) < platform_data.get_screen_blocks_height() )
						{
							block_x = ( ( block_n & 0x01 ) == 0x01 ? _scr_height_blocks_mul4:0 );
							block_y = block_n & 0x02;
							
							for( chr_n = 0; chr_n < 4; chr_n++ )
							{
								chr_x	= ( ( chr_n & 0x01 ) == 0x01 ? _scr_height_blocks_mul2:0 );
								chr_y	= ( chr_n & 0x02 ) >> 1;
								
								// column order by default
								_attrs_chr[ tile_x + tile_y + block_x + block_y + chr_x + chr_y ] = get_screen_attribute( _tiles, tile_id, block_n, chr_n );
							}
						}
					}
				}
			}
			else
			{
				for( block_n = 0; block_n < platform_data.get_screen_blocks_cnt(); block_n++ )
				{
					block_x = ( ( block_n / platform_data.get_screen_blocks_height() ) << 1 ) * _scr_height_blocks_mul2;
					block_y = ( block_n % platform_data.get_screen_blocks_height() ) << 1;

					for( chr_n = 0; chr_n < 4; chr_n++ )
					{
						chr_x	= ( ( chr_n & 0x01 ) == 0x01 ? _scr_height_blocks_mul2:0 );
						chr_y	= ( chr_n & 0x02 ) >> 1;
						
						// column order by default
						_attrs_chr[ block_x + block_y + chr_x + chr_y ] = get_screen_attribute( _tiles, _block_inds.get_tile( block_n ), chr_n );
					}
				}
			}
		
			if( RBtnTilesDirRows.Checked || _force_swapping )
			{
				utils.swap_columns_rows_order< ushort >( _attrs_chr, _scr_width_blocks_mul2, _scr_height_blocks_mul2 );
			}
		}
		
		private ushort get_screen_attribute( tiles_data _tiles, int _tile_id, int _block_n, int _chr_n )
		{
			return get_screen_attribute( _tiles, _tiles.get_tile_block( _tile_id, _block_n ), _chr_n );
		}
		
		private ushort get_screen_attribute( tiles_data _tiles, int _block_n, int _chr_n )
		{
			uint block_data = _tiles.blocks[ ( _block_n << 2 ) + _chr_n ];
			
			return get_screen_attribute( block_data );
		}
		
		private ushort get_screen_attribute( uint _block_data )
		{
			return ( ushort )tiles_data.block_data_repack_to_native( _block_data, get_CHR_offset() << 1 );
		}
		
		private string get_adjacent_screen_index( int _level_n, layout_data _data, int _scr_ind, int _x_offset, int _y_offset )
		{
			int adj_scr_ind = _data.get_adjacent_screen_index( _scr_ind, _x_offset, _y_offset, true );
			
			if( adj_scr_ind > 255 )
			{
				throw new Exception( "Layout: " + _level_n + " error!\nThe maximum number of cells in a layout must be 256 for the \"Adjacent Screen Indices\" mode!" );
			}
			
			return utils.hex( "$", ( adj_scr_ind >= 0 ? adj_scr_ind:255 ) );
		}
		
		private int get_CHR_offset()
		{
			return m_CHR_offset[ ComboBoxBAT.SelectedIndex ] + ( int )NumericUpDownCHROffset.Value;
		}
		
		private string get_adjacent_screen_label( int _level_n, layout_data _data, int _scr_ind, int _x_offset, int _y_offset )
		{
			int adj_scr_ind = _data.get_adjacent_screen_index( _scr_ind, _x_offset, _y_offset );
			
			return ( adj_scr_ind >= 0 ? m_level_prefix + _level_n + "Scr" + adj_scr_ind:"0" );
		}
		
		private bool check_screen_layouts( int _scr_ind )
		{
			layout_data data;
			
			int n_scr_X;
			int n_scr_Y;
			
			for( int layout_n = 0; layout_n < m_data_mngr.layouts_data_cnt; layout_n++ )
			{
				data = m_data_mngr.get_layout_data( layout_n );
				
				n_scr_X = data.get_width();
				n_scr_Y = data.get_height();
				
				for( int scr_n_X = 0; scr_n_X < n_scr_X; scr_n_X++ )
				{
					for( int scr_n_Y = 0; scr_n_Y < n_scr_Y; scr_n_Y++ )
					{
						if( data.get_data( scr_n_X, scr_n_Y ).m_scr_ind == _scr_ind )
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}
		
		private void save_multidir_scroll_mode( StreamWriter _sw )
		{
			BinaryWriter 	bw			= null;
			BinaryWriter 	bw_Tiles	= null;
			BinaryWriter 	bw_Attrs	= null;
			BinaryWriter 	bw_Props	= null;
			BinaryWriter 	bw_Maps		= null;
			BinaryWriter 	bw_MapsTbl	= null;
			BinaryWriter 	bw_Plts		= null;
			
			StreamWriter	uni_stream	= ( m_C_writer != null ) ? m_C_writer:_sw;
			
			layout_data level_data = null;
			
			byte[]	map_data_arr		= null;
			byte[]	map_tiles_arr		= null;
			byte[]	map_blocks_arr		= null;
			byte[]	block_props_arr		= null;

			List< tiles_data > scr_tiles_data = m_data_mngr.get_tiles_data();
			
			int n_levels 			= m_data_mngr.layouts_data_cnt;
			int n_scr_X 			= 0;
			int n_scr_Y				= 0;
			int tile_n				= 0;
			int block_n				= 0;
			int n_Y_tiles			= 0;
			int n_screens 			= 0;
			int scr_ind				= 0;
			int bank_ind			= 0;
			int chk_bank_ind		= 0;
			int n_banks				= scr_tiles_data.Count;
			int tile_offs_x			= 0;
			int tile_offs_y			= 0;
			int max_tile_ind 		= 0;
			int max_block_ind 		= 0;
			int blocks_props_size	= 0;
			ushort tile_id			= 0;
			byte block_id			= 0;
			uint block_data			= 0;
			
			layout_screen_data	scr_data;
			
			long 	data_size 		= 0;
			long	CHR_data_size	= 0;
			long	exp_data_size	= 0;
			
			string 	label 		= null;
			
			string level_prefix_str	= null;

			string c_data_prefix		= get_C_data_prefix();
			string c_data_prefix_no_exp = ( m_C_writer != null ) ? skip_exp_pref( c_data_prefix ):c_data_prefix;
			
			string CHRs_incbins		= "";
			string CHRs_size		= c_data_prefix + "CHRs_size:\n";
			string CHRs_arr			= "";
			string tiles_offs		= c_data_prefix + "TilesOffs:\n";
			string blocks_offs		= c_data_prefix + "BlocksOffs:\n";
			string props_offs		= c_data_prefix + "PropsOffs:\n";
			string maps_offs		= c_data_prefix + "MapsOffs:\n";
			string mapstbl_offs		= c_data_prefix + "MapsTblOffs:\n";
			string maps_arr			= c_data_prefix + "MapsArr:\n";
			string start_scr_arr	= c_data_prefix + "StartScrArr:\n";
			string map_dim_arr		= c_data_prefix + "MapsDimArr:\n";
			
			int scr_width_blocks 	= platform_data.get_screen_blocks_width();
			int scr_height_blocks 	= platform_data.get_screen_blocks_height();
			
			tiles_data tiles = null;

			for( int level_n = 0; level_n < n_levels; level_n++ )
			{
				level_data = m_data_mngr.get_layout_data( level_n );
				
				level_prefix_str = CONST_FILENAME_LEVEL_PREFIX + level_n;
				
				check_ent_instances_cnt( level_data, level_prefix_str );
				
				n_scr_X = level_data.get_width();
				n_scr_Y = level_data.get_height();

				n_Y_tiles = n_scr_Y * platform_data.get_screen_tiles_height() * ( RBtnTiles4x4.Checked ? 1:2 );
				
				// game level tilemap analysing
				{
					map_tiles_arr = new byte[ n_scr_X * n_scr_Y * platform_data.get_screen_tiles_cnt() ];
					
					Array.Clear( map_tiles_arr, 0, map_tiles_arr.Length );
				}
				
				if( RBtnTiles2x2.Checked )
				{
					map_blocks_arr = new byte[ n_Y_tiles * n_scr_X * ( platform_data.get_screen_width_pixels() >> 4 ) ];
					
					Array.Clear( map_blocks_arr, 0, map_blocks_arr.Length );
				}
				
				chk_bank_ind = -1;
				
				for( int scr_n_X = 0; scr_n_X < n_scr_X; scr_n_X++ )
				{
					for( int scr_n_Y = 0; scr_n_Y < n_scr_Y; scr_n_Y++ )
					{
						scr_data = level_data.get_data( scr_n_X, scr_n_Y );
						
						if( scr_data.m_scr_ind != layout_data.CONST_EMPTY_CELL_ID )
						{
							// determine which bank the screen belongs to
							n_screens = 0;
							for( bank_ind = 0; bank_ind < n_banks; bank_ind++ )
							{
								n_screens += scr_tiles_data[ bank_ind ].screen_data_cnt();
								
								if( scr_data.m_scr_ind < n_screens )
								{
									break;
								}
							}
							
							if( chk_bank_ind >= 0 && bank_ind != chk_bank_ind )
							{
								_sw.Close();
								throw new System.Exception( String.Format( "Each level must use ONLY one CHR bank in the 'Multidirectional scrolling' mode!\n\nThe level_{0} export failed!", level_n ) );
							}
							
							chk_bank_ind = bank_ind;
							
							// convert a screen index into local index in the bank
							scr_ind = scr_data.m_scr_ind - ( n_screens - scr_tiles_data[ bank_ind ].screen_data_cnt() );
							
							// fill the map by tiles of a current screen
							tiles = scr_tiles_data[ bank_ind ];
							
							if( m_data_mngr.screen_data_type == data_sets_manager.EScreenDataType.sdt_Tiles4x4 )
							{
								for( tile_n = 0; tile_n < platform_data.get_screen_tiles_cnt(); tile_n++ )
								{
									tile_offs_x = ( tile_n % platform_data.get_screen_tiles_width() );
									tile_offs_y = ( tile_n / platform_data.get_screen_tiles_width() );
									
									tile_id = tiles.get_screen_tile( scr_ind, tile_n );
									
									if( RBtnTiles2x2.Checked )
									{
										map_tiles_arr[ scr_n_X * ( ( n_scr_Y * platform_data.get_screen_tiles_height() ) * platform_data.get_screen_tiles_width() ) + ( scr_n_Y * platform_data.get_screen_tiles_height() ) + ( tile_offs_x * ( n_scr_Y * platform_data.get_screen_tiles_height() ) ) + tile_offs_y ] = ( byte )tile_id;
										
										tile_offs_x <<= 1;
										tile_offs_y <<= 1;
										
										// make a list of 2x2 tiles of the current map
										for( block_n = 0; block_n < utils.CONST_TILE_SIZE; block_n++ )
										{
											block_id = ( byte )tiles.get_tile_block( tile_id, block_n );
											
											if( ( tile_offs_x + ( block_n & 0x01 ) ) < platform_data.get_screen_blocks_width() && ( tile_offs_y + ( ( block_n & 0x02 ) >> 1 ) ) < platform_data.get_screen_blocks_height() )
											{
												map_blocks_arr[ scr_n_X * ( n_Y_tiles * scr_width_blocks ) + ( scr_n_Y * scr_height_blocks ) + ( tile_offs_x * n_Y_tiles ) + ( ( block_n & 0x01 ) == 0x01 ? n_Y_tiles:0 ) + tile_offs_y + ( ( block_n & 0x02 ) == 0x02 ? 1:0 ) ] = block_id;
											}
										}
									}
									else
									{
										map_tiles_arr[ scr_n_X * ( n_Y_tiles * platform_data.get_screen_tiles_width() ) + ( scr_n_Y * platform_data.get_screen_tiles_height() ) + ( tile_offs_x * n_Y_tiles ) + tile_offs_y ] = ( byte )tile_id;
									}
								}
							}
							else
							{
								// make a list of 2x2 tiles of the current map
								for( block_n = 0; block_n < platform_data.get_screen_blocks_cnt(); block_n++ )
								{
									tile_offs_x = ( block_n % platform_data.get_screen_blocks_width() );
									tile_offs_y = ( block_n / platform_data.get_screen_blocks_width() );
									
									map_blocks_arr[ scr_n_X * ( n_Y_tiles * scr_width_blocks ) + ( scr_n_Y * scr_height_blocks ) + ( tile_offs_x * n_Y_tiles ) + tile_offs_y ] = ( byte )tiles.get_screen_tile( scr_ind, block_n );
								}
							}
						}
					}
				}

				uni_stream.WriteLine( "\n" + ( m_C_writer != null ? "//":";" ) + " *** " + level_prefix_str + " ***" );
				
				// write collected data
				level_prefix_str = m_level_prefix + level_n;

				maps_arr += "\t.word " + level_prefix_str + "_Layout\n";
				
				tiles = scr_tiles_data[ chk_bank_ind ];

				// write CHR banks data
				label = get_exp_prefix() + "chr" + level_n;
				bw = new BinaryWriter( File.Open( m_path + m_filename + "_" + label + CONST_BIN_EXT, FileMode.Create ) );
				{
					tiles.export_CHR( bw );
				}
				CHR_data_size = data_size = bw.BaseStream.Length;
				bw.Close();
				
				exp_data_size += data_size;
				
				CHRs_incbins	+= label + ":\t.incbin \"" + m_filename + "_" + label + CONST_BIN_EXT + "\"\t\t; (" + data_size + ")\n";
				CHRs_size		+= "\t.word " + data_size + "\t; (" + label + ")\n";
			
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "\nextern u16*\t" + skip_exp_pref( label ) + ";" );
					
					CHRs_arr += "\t.word bank(" + label + ")\n";
				}

				CHRs_arr += "\t.word " + label + "\n";

				if( m_data_mngr.screen_data_type == data_sets_manager.EScreenDataType.sdt_Tiles4x4 )
				{
					max_tile_ind = 1 + map_tiles_arr.Max();	// one based index
				}
				else
				{
					max_tile_ind = tiles.get_first_free_block_id();
					max_tile_ind = ( max_tile_ind < 0 ) ? platform_data.get_max_blocks_cnt():max_tile_ind;
				}
				
				if( RBtnTiles4x4.Checked )
				{
					// write tiles
					if( bw_Tiles == null )
					{
						bw_Tiles = new BinaryWriter( File.Open( m_path_filename + "_Tiles" + CONST_BIN_EXT, FileMode.Create ) );
					}

					tiles_offs += "\t.word " + bw_Tiles.BaseStream.Length + "\t; (chr" + level_n + ")\n";
					
					for( int i = 0; i < max_tile_ind; i++ )
					{
						bw_Tiles.Write( tiles_data.tile_convert_ulong_to_uint_reverse( tiles.tiles[ i ] ) );
					}
					
					exp_data_size += bw_Tiles.BaseStream.Length;
				}
				
				// tiles 2x2
				{
					max_block_ind = tiles.get_first_free_block_id();
					max_block_ind = ( max_block_ind < 0 ? platform_data.get_max_blocks_cnt():max_block_ind ) << 2;	// 4 ushorts per block
					
					label = "_Attrs";
					
					if( bw_Attrs == null )
					{
						bw_Attrs = new BinaryWriter( File.Open( m_path_filename + label + CONST_BIN_EXT, FileMode.Create ) );
					}
	
					blocks_offs += "\t.word " + bw_Attrs.BaseStream.Length + "\t; (chr" + level_n + ")\n";
						
					for( block_n = 0; block_n < max_block_ind; block_n++ )
					{
						block_data = tiles.blocks[ block_n ];
						
						bw_Attrs.Write( get_screen_attribute( block_data ) );
					}
					
					exp_data_size += bw_Attrs.BaseStream.Length;
				}
				
				// blocks and properties
				{
					if( m_data_mngr.screen_data_type == data_sets_manager.EScreenDataType.sdt_Tiles4x4 )
					{
						blocks_props_size = ( 1 + utils.get_ulong_arr_max_val( tiles.tiles, max_tile_ind ) ) << 2;
					}
					else
					{
						blocks_props_size = tiles.get_first_free_block_id();
						blocks_props_size = ( ( blocks_props_size < 0 ) ? platform_data.get_max_blocks_cnt():blocks_props_size ) << 2;
					}
					
					block_props_arr = new byte[ RBtnPropPerBlock.Checked ? ( blocks_props_size >> 2 ):blocks_props_size ];
					Array.Clear( block_props_arr, 0, block_props_arr.Length );
					
					for( block_n = 0; block_n < blocks_props_size; block_n++ )
					{
						block_data = tiles.blocks[ block_n ];
						
						if( RBtnPropPerBlock.Checked && ( block_n % 4 ) != 0 )
						{
							continue;
						}
						
						block_props_arr[ RBtnPropPerBlock.Checked ? ( ( block_n + 1 ) >> 2 ):block_n ]	= (byte)tiles_data.get_block_flags_obj_id( block_data );
					}
					
					// save properties
					label = "_Props";
					
					if( bw_Props == null )
					{
						bw_Props = new BinaryWriter( File.Open( m_path_filename + label + CONST_BIN_EXT, FileMode.Create ) );
					}
					
					props_offs += "\t.word " + bw_Props.BaseStream.Length + "\t; (chr" + level_n + ")\n";
					
					bw_Props.Write( block_props_arr );
					
					exp_data_size += bw_Props.BaseStream.Length;
				}

				// write map
				{
					label = "_Maps";
					
					if( bw_Maps == null )
					{
						bw_Maps = new BinaryWriter( File.Open( m_path_filename + label + CONST_BIN_EXT, FileMode.Create ) );
					}
					
					maps_offs += "\t.word " + bw_Maps.BaseStream.Length + "\n";
					
					if( RBtnTiles2x2.Checked )
					{
						map_data_arr = map_blocks_arr;
					}
					else
					{
						map_data_arr = map_tiles_arr;
					}
					
					if( RBtnTilesDirRows.Checked )
					{
						utils.swap_columns_rows_order<byte>( map_data_arr, get_tiles_cnt_width( n_scr_X ), get_tiles_cnt_height( n_scr_Y ) );
					}
					
					if( compress_and_save_byte( bw_Maps, map_data_arr ) == false )
					{
						_sw.Close();
						bw_Maps.Close();
						throw new System.Exception( "Can't compress an empty data!" );
					}
					
					exp_data_size += bw_Maps.BaseStream.Length;
				}
				
				// tiles lookup table
				{
					label = "_MapsTbl";
					
					if( bw_MapsTbl == null )
					{
						bw_MapsTbl = new BinaryWriter( File.Open( m_path_filename + label + CONST_BIN_EXT, FileMode.Create ) );
					}
					
					mapstbl_offs += "\t.word " + bw_MapsTbl.BaseStream.Length + "\n";
					
					int w_tiles_cnt = get_tiles_cnt_width( n_scr_X );
					int h_tiles_cnt = get_tiles_cnt_height( n_scr_Y );
					
					if( RBtnTilesDirColumns.Checked )
					{
						for( int i = 0; i < w_tiles_cnt; i++ )
						{
							bw_MapsTbl.Write( ( ushort )( i * h_tiles_cnt ) );
						}
					}
					else
					{
						for( int i = 0; i < h_tiles_cnt; i++ )
						{
							bw_MapsTbl.Write( ( ushort )( i * w_tiles_cnt ) );
						}
					}
					
					exp_data_size += bw_MapsTbl.BaseStream.Length;
				}
				
				// palettes
				{
					label = "_Plts";
					
					if( bw_Plts == null )
					{
						bw_Plts = new BinaryWriter( File.Open( m_path_filename + label + CONST_BIN_EXT, FileMode.Create ) );
					}
					
					for( int i = 0; i < tiles.palettes_arr.Count; i++ )
					{
						utils.write_int_as_ushort_arr( bw_Plts, tiles.palettes_arr[ i ].m_palette0 );
						utils.write_int_as_ushort_arr( bw_Plts, tiles.palettes_arr[ i ].m_palette1 );
						utils.write_int_as_ushort_arr( bw_Plts, tiles.palettes_arr[ i ].m_palette2 );
						utils.write_int_as_ushort_arr( bw_Plts, tiles.palettes_arr[ i ].m_palette3 );
					}
					
					exp_data_size += bw_Plts.BaseStream.Length;
				}				
/*				
				palette_str = level_prefix_str + "_Palette:";
			
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "extern u16*\t" + skip_exp_pref( level_prefix_str ) + "_Palette;" );
				}
				
				for( int i = 0; i < tiles.palettes_arr.Count; i++ )
				{
					palette_str += "\n\t\t.word ";
					
					fill_palette_str( tiles.palettes_arr[ i ].m_palette0, ref palette_str, false );
					fill_palette_str( tiles.palettes_arr[ i ].m_palette1, ref palette_str, false );
					fill_palette_str( tiles.palettes_arr[ i ].m_palette2, ref palette_str, false );
					fill_palette_str( tiles.palettes_arr[ i ].m_palette3, ref palette_str, true );
				}
				
				_sw.WriteLine( palette_str + "\n" + ( level_prefix_str + "_Palette_End:\n" ) );
*/
				int start_scr_ind = level_data.get_start_screen_ind();
				
				if( start_scr_ind < 0 )
				{
					MainForm.message_box( "The start screen wasn't assigned to layout: " + level_n + "\n\nWARNING: A zero screen will be used as a start one.", "Start Screen Warning", MessageBoxButtons.OK );
					
					start_scr_ind = 0;
				}

				start_scr_arr	+= "\t.byte " + level_prefix_str + "_StartScr\n";
				map_dim_arr		+= "\t.byte " + level_prefix_str + "_WScrCnt, " + level_prefix_str + "_HScrCnt" + "\n";
				
				StreamWriter def_sw = _sw;
				
				string c_char		= "";
				string c_int		= "";
				string c_code_comm_delim = "\t; ";
				
				if( m_C_writer != null )
				{
					level_prefix_str 	= skip_exp_pref( level_prefix_str );
					c_char				= "const u8\t";
					c_int				= "const u16\t";
					c_code_comm_delim 	= ";\t// ";
					
					def_sw	= m_C_writer;
				}

				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "extern u8\t" + level_prefix_str + "_StartScr" + c_code_comm_delim + "start screen" );
				}
				else
				{
					def_sw.WriteLine( c_int + level_prefix_str + "_CHR_data_size\t= " + CHR_data_size + c_code_comm_delim + "map CHRs size in bytes" );
					
					def_sw.WriteLine( c_int + level_prefix_str + "_WTilesCnt\t= " + get_tiles_cnt_width( n_scr_X ) + c_code_comm_delim + "number of level tiles in width" );
					def_sw.WriteLine( c_int + level_prefix_str + "_HTilesCnt\t= " + get_tiles_cnt_height( n_scr_Y ) + c_code_comm_delim + "number of level tiles in height" );
					
					def_sw.WriteLine( c_int + level_prefix_str + "_WPixelsCnt\t= " + get_tiles_cnt_width( n_scr_X ) * ( RBtnTiles2x2.Checked ? 16:32 ) + c_code_comm_delim + "map width in pixels" );
					def_sw.WriteLine( c_int + level_prefix_str + "_HPixelsCnt\t= " + get_tiles_cnt_height( n_scr_Y ) * ( RBtnTiles2x2.Checked ? 16:32 ) + c_code_comm_delim + "map height in pixels" );
					def_sw.WriteLine( c_char + level_prefix_str + "_BlocksCnt\t= " + ( max_block_ind >> 2 ) + c_code_comm_delim + "map blocks count\n" );
				}
				
				if( RBtnTiles4x4.Checked )
				{
					def_sw.WriteLine( c_char + level_prefix_str + "_TilesCnt\t= " + max_tile_ind + c_code_comm_delim + "map tiles count" );
				}
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "\n#asm" );
				}

				def_sw.WriteLine( get_exp_prefix() + level_prefix_str + "_StartScr\t= " + start_scr_ind + "\t; start screen" );
				
				level_data.export_asm( uni_stream, ( get_exp_prefix() + level_prefix_str ), null, ".byte", ".word", ".word", "$", true, CheckBoxExportMarks.Checked, CheckBoxExportEntities.Checked, RBtnEntityCoordScreen.Checked, ( m_C_writer != null ? false:true ) );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "#endasm\n" );
					
					if( CheckBoxExportEntities.Checked )
					{
						m_C_writer.WriteLine( "extern u16\t" + level_prefix_str + "_EntInstCnt;\t// number of entities instances" );
					}
				}
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "extern mpd_SCREEN*\t" + level_prefix_str + "_Layout[];\n" );
					m_C_writer.WriteLine( "extern u8\t" + level_prefix_str + "_WScrCnt;\t// number of screens in width" );
					m_C_writer.WriteLine( "extern u8\t" + level_prefix_str + "_HScrCnt;\t// number of screens in height" );
				}
				
				map_data_arr 	= null;
				map_tiles_arr 	= null;
				map_blocks_arr	= null;
				block_props_arr	= null;
			}
			
			_sw.WriteLine( "; *** GLOBAL DATA ***\n" );
			
			if( m_C_writer != null )
			{
				m_C_writer.WriteLine( "\n// *** GLOBAL DATA ***" );
			}
			
			_sw.WriteLine( CHRs_incbins );
			_sw.WriteLine( "\n" + CHRs_size );

			// CHRs array
			if( m_C_writer != null )
			{
				m_C_writer.WriteLine( "\n#asm\n" + c_data_prefix + "CHRs:\n" + CHRs_arr + "#endasm\n\nextern u16\t" + c_data_prefix_no_exp + "CHRs[];" );
				m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "CHRs_size;" );
			}
			else
			{
				_sw.WriteLine( m_filename + "_CHRs:\n" + CHRs_arr );
			}
			
			if( bw_Tiles != null )
			{
				_sw.WriteLine( c_data_prefix + "Tiles" + ":\t.incbin \"" + m_filename + "_Tiles" + CONST_BIN_EXT + "\"\t; (" + bw_Tiles.BaseStream.Length + ") (4x4) 4 block indices per tile ( left to right, up to down ) of all exported data banks\n" );
				
				_sw.WriteLine( tiles_offs );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "extern u8*\t" + c_data_prefix_no_exp + "Tiles" + ";" );
					m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "TilesOffs" + ";" );
				}
				
				bw_Tiles.Dispose();
			}
			
			if( bw_Attrs != null )
			{
				_sw.WriteLine( c_data_prefix + "Attrs" + ":\t.incbin \"" + m_filename + "_Attrs" + CONST_BIN_EXT + "\"\t; " + "(" + bw_Attrs.BaseStream.Length + ") attributes array per block ( 2 bytes per attribute; 8 bytes per block ) of all exported data banks\n" );
				
				_sw.WriteLine( blocks_offs );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "Attrs" + ";" );
					m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "BlocksOffs" + ";" );
				}
				
				bw_Attrs.Dispose();
			}
			
			if( bw_Props != null )
			{
				_sw.WriteLine( c_data_prefix + "Props" + ":\t.incbin \"" + m_filename + "_Props" + CONST_BIN_EXT + "\"\t; (" + bw_Props.BaseStream.Length + ") blocks properties array ( " + ( RBtnPropPerCHR.Checked ? "4 bytes":"1 byte" ) + " per block ) of all exported data banks\n" );
				
				if( m_C_writer != null )
				{
					props_offs += "\t.word " + bw_Props.BaseStream.Length + "\t; data end\n";
					
					m_C_writer.WriteLine( "extern u8*\t" + c_data_prefix_no_exp + "Props" + ";" );
					m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "PropsOffs" + ";" );
				}

				_sw.WriteLine( props_offs );
				
				align_word( bw_Props );
				bw_Props.Dispose();
			}

			if( bw_Maps != null )
			{
				_sw.WriteLine( c_data_prefix + "Maps" + ":\t.incbin \"" + m_filename + "_Maps" + CONST_BIN_EXT + "\"\t; " + ( CheckBoxRLE.Checked ? "compressed ":"" ) + "(" + bw_Maps.BaseStream.Length + ") game levels " + ( RBtnTiles4x4.Checked ? "tiles (4x4)":"blocks (2x2)" ) + " array of all exported data banks\n" );
				
				_sw.WriteLine( maps_offs );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "extern u8*\t" + c_data_prefix_no_exp + "Maps" + ";" );
					m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "MapsOffs" + ";" );
				}
				
				bw_Maps.Dispose();
			}
			
			if( bw_MapsTbl != null )
			{
				_sw.WriteLine( c_data_prefix + "MapsTbl" + ":\t.incbin \"" + m_filename + "_MapsTbl" + CONST_BIN_EXT + "\"\t; (" + bw_MapsTbl.BaseStream.Length + ") lookup table for fast calculation of tile addresses; " + ( RBtnTilesDirColumns.Checked ? "columns by X coordinate":"rows by Y coordinate" ) + " ( 16 bit offset per " + ( RBtnTilesDirColumns.Checked ? "column":"row" ) + " of tiles ) of all exported data banks\n" );
				
				_sw.WriteLine( mapstbl_offs );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "MapsTbl" + ";" );
					m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "MapsTblOffs" + ";" );
				}
				
				bw_MapsTbl.Dispose();
			}
			
			if( bw_Plts != null )
			{
				_sw.WriteLine( c_data_prefix + "Plts" + ":\t.incbin \"" + m_filename + "_Plts" + CONST_BIN_EXT + "\"\t; (" + bw_Plts.BaseStream.Length + ") palettes array of all exported data banks ( data offset = chr_id * " + ( ( platform_data.get_fixed_palette16_cnt() << 4 ) << 1 ) + " )\n" );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "extern u16*\t" + c_data_prefix_no_exp + "Plts" + ";" );
				}
				
				bw_Plts.Dispose();
			}

			// save MapsArr
			{
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "\nextern mpd_SCREEN** " + c_data_prefix_no_exp + "MapsArr[];" );
					m_C_writer.WriteLine( "\n#asm" );
				}
				
				uni_stream.WriteLine( maps_arr );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "#endasm" );
				}
			}
			
			// start screens array
			{
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "\nextern u8 " + c_data_prefix_no_exp + "StartScrArr[];" );
					m_C_writer.WriteLine( "\n#asm" );
				}
				
				uni_stream.WriteLine( start_scr_arr );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "#endasm" );
				}
			}
			
			// map dimensions array
			{
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "\nextern u8 " + c_data_prefix_no_exp + "MapsDimArr[];" );
					m_C_writer.WriteLine( "\n#asm" );
				}
				
				uni_stream.WriteLine( map_dim_arr );
				
				if( m_C_writer != null )
				{
					m_C_writer.WriteLine( "#endasm" );
				}
			}

			if( exp_data_size > 8192 )
			{
				MainForm.message_box( "The exported binary data size exceeds 8K ( " + exp_data_size + " B ) !", "Data Export Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning );
			}
		}
		
		int get_tiles_cnt_width( int _scr_cnt_x )
		{
			return RBtnTiles2x2.Checked ? _scr_cnt_x * platform_data.get_screen_blocks_width():_scr_cnt_x * platform_data.get_screen_tiles_width();
		}

		int get_tiles_cnt_height( int _scr_cnt_y )
		{
			return RBtnTiles2x2.Checked ? _scr_cnt_y * platform_data.get_screen_blocks_height():_scr_cnt_y * platform_data.get_screen_tiles_height();
		}
		
		void fill_palette_str( int[] _plt, ref string _str, bool _end )
		{
			for( int j = 0; j < utils.CONST_PALETTE_SMALL_NUM_COLORS; j++ )
			{
				_str += String.Format( "${0:X2}", _plt[ j ] ) + ( !( _end && j == 3 ) ? ", ":"" );
			}
		}
		
		void check_ent_instances_cnt( layout_data _layout, string _lev_pref_str )
		{
			if( CheckBoxExportEntities.Checked )
			{
				if( _layout.get_ent_instances_cnt() > utils.CONST_MAX_ENT_INST_CNT )
				{
					throw new Exception( "The number of entity instances is out of range!\nThe maximum number allowed to export: " + utils.CONST_MAX_ENT_INST_CNT + "\n\n[" + _lev_pref_str + "]" );
				}
			}
		}
	}
}