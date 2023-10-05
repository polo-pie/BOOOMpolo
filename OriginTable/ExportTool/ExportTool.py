import os
import pandas as pd
import tkinter as tk
from tkinter import ttk
import subprocess

def get_git_root_path():
    try:
        root = subprocess.check_output(['git', 'rev-parse', '--show-toplevel']).strip().decode('utf-8')
    except subprocess.CalledProcessError:
        print("Error: Not a git repository")
        return None
    return root

def export_excel_to_csv():
    try:
        git_root = get_git_root_path()

        if git_root:
            excel_dir = os.path.normpath(os.path.join(git_root, 'OriginTable'))
            csv_dir = os.path.normpath(os.path.join(git_root, 'MeowDice', 'Assets', 'Resources', 'Table'))
            
            print(excel_dir)
            print(csv_dir)
            
            # 确保CSV目录存在
            if not os.path.exists(csv_dir):
                os.makedirs(csv_dir)

            # 遍历Excel目录，找到所有Excel文件
            for filename in os.listdir(excel_dir):
                if filename.endswith('.xlsx') or filename.endswith('.xls'):
                    # 读取Excel文件
                    excel_path = os.path.join(excel_dir, filename)
                    xls = pd.ExcelFile(excel_path)
                    
                    # 遍历Excel中的所有sheet
                    for sheet_name in xls.sheet_names:
                        df = pd.read_excel(excel_path, sheet_name=sheet_name)
                        
                        # 保存为CSV文件
                        csv_filename = f"{sheet_name}.csv"
                        csv_path = os.path.join(csv_dir, csv_filename)
                        df.to_csv(csv_path, index=False)

            success_label.config(text="导表成功")
        else:
            success_label.config(text="导表失败，不是一个Git仓库")
        
    except Exception as e:
        print(e)  # 输出错误信息到控制台，方便调试
        success_label.config(text="导表失败")

# 创建Tkinter窗口
root = tk.Tk()
root.title("Excel to CSV Export Tool")
root.geometry("300x200")  # 设置窗口大小

# 添加按钮
export_button = ttk.Button(root, text="导表", command=export_excel_to_csv)
export_button.pack(pady=20)

# 添加成功标签
success_label = ttk.Label(root, text="")
success_label.pack(pady=10)

# 运行Tkinter事件循环
root.mainloop()