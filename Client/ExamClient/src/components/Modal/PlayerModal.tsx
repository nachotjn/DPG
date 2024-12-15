import axios from 'axios';
import { ChangeEventHandler, useEffect, useState } from 'react';
import { Button, Form, Modal } from "react-bootstrap";

interface Player {
  id: number;
  name: string;
  email: string;
  phone: string;
  password: string;
}

export const PlayerModal = ({ showModal, setModal, editPlayer }: {showModal: boolean, setModal: (visible: boolean) => void, editPlayer?: Player}) => {
    const [playerInfo, setPlayerInfo] = useState<Player>();

  const handleName = (e: any) => {
    console.log("Name:", e.target.value);
    setPlayerInfo((prevState: any) => {
        return ({
            ...prevState,
            name: e.target.value
        })
    })
  };

  const handleEmail = (e: any) => {
    console.log("Email:", e.target.value);
    setPlayerInfo((prevState: any) => {
        return ({
            ...prevState,
            email: e.target.value
        })
    })
  };

  const handlePhone = (e: any) => {
    console.log("Phone:", e.target.value);
    setPlayerInfo((prevState: any) => {
        return ({
            ...prevState,
            phone: e.target.value
        })
    })
  };

  const handlePassword = (e: any) => {
    console.log("PW:", e.target.value);
    setPlayerInfo((prevState: any) => {
        return ({
            ...prevState,
            password: e.target.value
        })
    })
  };

  const handleSubmit = () => {
    console.log('Sending info to server...')
    console.log('New Player state:', playerInfo)
    axios.post('/api/PlayerController', playerInfo)
  }

  return (
    <Modal show={showModal} onHide={() => setModal(!showModal)}>
      <Modal.Header closeButton>
        <Modal.Title>New Player</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form onSubmit={handleSubmit}>
          <Form.Group className="mb-3" controlId="formBasicName">
            <Form.Label>Name</Form.Label>
            <Form.Control defaultValue={editPlayer?.name} onChange={handleName} required type="text" placeholder="Enter name" />
          </Form.Group>

          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Email</Form.Label>
            <Form.Control defaultValue={editPlayer?.email} onChange={handleEmail} required type="email" placeholder="Enter email" />
          </Form.Group>

          <Form.Group className="mb-3" controlId="formBasicPhone">
            <Form.Label>Phone</Form.Label>
            <Form.Control defaultValue={editPlayer?.phone} onChange={handlePhone} required type="number" placeholder="Enter phone number" />
          </Form.Group>

          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label>Password</Form.Label>
            <Form.Control defaultValue={editPlayer?.password} onChange={handlePassword} required type="password" placeholder="Password" />
          </Form.Group>

          <Button variant="primary" type="submit">
            Submit
          </Button>
        </Form>
      </Modal.Body>
    </Modal>
  );
};
