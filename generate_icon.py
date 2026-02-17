from PIL import Image
import os

def convert_png_to_ico(source_path, target_path):
    if not os.path.exists(source_path):
        print(f"Error: {source_path} not found.")
        return

    img = Image.open(source_path)
    
    # Standard ICO sizes for Windows
    icon_sizes = [(16, 16), (24, 24), (32, 32), (48, 48), (64, 64), (128, 128), (256, 256)]
    
    # Save as ICO with all resolutions
    img.save(target_path, format='ICO', sizes=icon_sizes)
    print(f"Successfully converted {source_path} to {target_path}")

if __name__ == "__main__":
    convert_png_to_ico("new_icon_source.png", "app_icon.ico")
