import React from 'react';
import CreatePlayerForm from './CreatePlayerForm';

interface CreatePlayerModalProps {
  isOpen: boolean;
  onClose: () => void;
  refreshPlayers: () => void; 
}

const CreatePlayerModal: React.FC<CreatePlayerModalProps> = ({ isOpen, onClose, refreshPlayers }) => {
  if (!isOpen) return null; 

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <button className="close-modal" onClick={onClose}>
          &times;
        </button>
        <CreatePlayerForm refreshPlayers={refreshPlayers} /> 
      </div>
    </div>
  );
};

export default CreatePlayerModal;
