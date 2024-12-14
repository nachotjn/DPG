import React from 'react';
import CreatePlayerForm from './CreatePlayerForm';

interface CreatePlayerModalProps {
  isOpen: boolean;
  onClose: () => void;
}

const CreatePlayerModal: React.FC<CreatePlayerModalProps> = ({ isOpen, onClose }) => {
  if (!isOpen) return null; 

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <button className="close-modal" onClick={onClose}>
          &times;
        </button>
        <CreatePlayerForm /> 
      </div>
    </div>
  );
};

export default CreatePlayerModal;
