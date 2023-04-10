using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$type, $left_offset, $right_offset, $top_offset, $bottom_offset, $position_mode, $fill_mode, $text, $image_path, $rotation, $color, $id
    public class SearchWatermarkDTO : AbstractSearchDTO
    {
        private string _type;
        private string _position_mode;
        private string _fill_mode;
        private string _text;
        private string _image_path;
        private string _color;

        public string type
        {
            get { return string.IsNullOrEmpty(_type) ? "" : _type; }
            set { _type = value; }
        }

        public int left_offset { get; set; } = 0;

        public int right_offset { get; set; } = 0;

        public int top_offset { get; set; } = 0;

        public int bottom_offset { get; set; } = 0;

        public string position_mode
        {
            get { return string.IsNullOrEmpty(_position_mode) ? "" : _position_mode; }
            set { _position_mode = value; }
        }

        public string fill_mode
        {
            get { return string.IsNullOrEmpty(_fill_mode) ? "" : _fill_mode; }
            set { _fill_mode = value; }
        }

        public string text
        {
            get { return string.IsNullOrEmpty(_text) ? "" : _text; }
            set { _text = value; }
        }

        public string image_path
        {
            get { return string.IsNullOrEmpty(_image_path) ? "" : _image_path; }
            set { _image_path = value; }
        }

        public float? rotation { get; set; } = 0;

        public string color
        {
            get { return string.IsNullOrEmpty(_color) ? "" : _color; }
            set { _color = value; }
        }

        public int id { get; set; } = 0;

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "type";
                case "1": return "left_offset";
                case "2": return "right_offset";
                case "3": return "top_offset";
                case "4": return "bottom_offset";
                case "5": return "position_mode";
                case "6": return "fill_mode";
                case "7": return "text";
                case "8": return "image_path";
                case "9": return "rotation";
                case "10": return "color";
                default: return "type";
            }
        }
    }
}