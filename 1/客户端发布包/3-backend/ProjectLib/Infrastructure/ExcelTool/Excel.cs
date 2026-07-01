using System.ComponentModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Reflection;
using System.Data;

namespace Infrastructure.ExcelTool;

public class Excel
{
    /// <summary>
    /// 从数据集合中读取数据到Excel数据流中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">数据集合</param>
    /// <param name="basePath">临时文件基础路径</param>
    /// <returns></returns>
    public string ReadListToExcel<T>(List<T> list, string basePath) where T : class
    {
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();
        properties = properties.Where(e => e.CanRead).ToArray();

        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet();
        MemoryStream stream = new();

        try
        {
            //写入表头
            IRow header = sheet.CreateRow(0);
            for (int i = 0; i < properties.Length; i++)
            {
                string cellName = "";
                ICell cell = header.CreateCell(i);
                DisplayNameAttribute? display = properties[i].GetCustomAttribute<DisplayNameAttribute>();
                if (display != null) { cellName = display.DisplayName; }
                else { cellName = properties[i].Name; }
                cell.SetCellValue(cellName);
            }
            //写入表内容
            for (int i = 0; i < list.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < properties.Length; j++)
                {
                    object? value = properties[j].GetValue(list[i], null);
                    row.CreateCell(j).SetCellValue(value?.ToString() ?? "");
                }
            }

            string fileName = $"temps/{DateTime.Now.Ticks}.xlsx";
            string filePath = Path.GetFullPath($"{basePath}{fileName}");
            using (FileStream fileStream = File.Create(filePath))
            {
                workbook.Write(fileStream, false);
            }
            _ = DeleteTempFile(filePath);

            return fileName;
        }
        catch { throw; }
    }

    /// <summary>
    /// 从Excel数据流中读取数据到DataTable中
    /// </summary>
    /// <param name="stream">Excel数据流</param>
    /// <returns></returns>
    public DataTable ReadExcelToDataTable(Stream stream)
    {
        DataTable table = new();

        try
        {
            IWorkbook workbook = WorkbookFactory.Create(stream);
            //读取当前表数据
            ISheet sheet = workbook.GetSheetAt(0);
            //读取第一行表头
            int firstRowNum = sheet.FirstRowNum;
            IRow row = sheet.GetRow(firstRowNum);
            foreach (ICell cell in row.Cells)
            {
                string value = cell.StringCellValue;
                table.Columns.Add(new DataColumn(value));
            }
            //读取表内容
            int endRowNum = sheet.LastRowNum;
            int colCount = table.Columns.Count;
            for (int i = firstRowNum + 1; i <= endRowNum; i++)
            {
                row = sheet.GetRow(i);
                DataRow tableRow = table.NewRow();

                for (int j = 0; j < colCount; j++)
                {
                    string? value = row.GetCell(j)?.ToString();
                    tableRow[j] = value;
                }

                table.Rows.Add(tableRow);
            }
        }
        catch { throw; }
        finally { stream.Close(); }

        return table;
    }

    /// <summary>
    /// 将数据集合导入到Excel模板
    /// </summary>
    /// <param name="list">数据集合</param>
    /// <param name="templatePath">模板文件路径</param>
    /// <param name="basePath">临时文件基础路径</param>
    /// <returns></returns>
    public string ExportExcelTemplate(List<string[]> list, string templatePath, string basePath)
    {
        try
        {
            MemoryStream tempStream = new();
            string templateFile = Path.GetFullPath(templatePath);
            using (FileStream fileStream = File.OpenRead(templateFile))
            {
                fileStream.CopyTo(tempStream);
            }

            IWorkbook workbook = WorkbookFactory.Create(tempStream);
            ISheet sheet = workbook.GetSheetAt(0);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null) { continue; }

                string[] infos = list[i];
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < infos.Length; j++)
                {
                    row.CreateCell(j).SetCellValue(infos[j]);
                }
            }

            string? extension = Path.GetExtension(templateFile);
            string fileName = $"temps/{DateTime.Now.Ticks}{extension}";
            string filePath = Path.GetFullPath($"{basePath}{fileName}");
            using (FileStream fileStream = File.Create(filePath))
            {
                workbook.Write(fileStream, false);
            }
            _ = DeleteTempFile(filePath);

            return fileName;
        }
        catch { throw; }
    }

    /// <summary>
    /// 删除临时文件
    /// </summary>
    /// <param name="filePath">临时文件路径</param>
    /// <returns></returns>
    private async Task DeleteTempFile(string filePath)
    {
        await Task.Delay(300000);
        File.Delete(filePath);
    }
}