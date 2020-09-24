using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLtoXLSXcvt
{
    static class ErrorMessenger
    {
        public static void ShowNoFileError(IWin32Window window, string file)
        {
            MessageBox.Show(window, "Файл [" + file + "] не существует.",
                "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowBadConfigError(IWin32Window window, string config, string line)
        {
            MessageBox.Show(window, "Неподходящий формат строки [" + line + "].",
                "Ошибка в конфигурации [" + config + "].", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowNoColumnsError(IWin32Window window)
        {
            MessageBox.Show(window, "Столбцы не выбраны.",
                "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowCanNotSaveError(IWin32Window window)
        {
            MessageBox.Show(window, "Невозможно сохранить результат конвертации. Возможно, этот файл открыт в другой программе.",
                "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowCanNotOpenError(IWin32Window window)
        {
            MessageBox.Show(window, "Невозможно открыть файл. Возможно, этот файл открыт в другой программе.",
                "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowBadFileNameError(IWin32Window window, string path)
        {
            MessageBox.Show(window, "Недопустимое имя файла [" + path + "].",
                "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}
