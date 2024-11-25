import { useState } from "react";

const UploadImageView = () => {
  const [image, setImage] = useState<File | null>(null); // Tipo File | null

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0] || null;
    setImage(file);
  };

  return (
    <div>
      <input type="file" onChange={handleFileChange} />
      {image && <p>File selected: {image.name}</p>}
    </div>
  );
};

export default UploadImageView;
