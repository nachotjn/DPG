import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import { Button, Form, Row, Col } from 'react-bootstrap';

interface Player {
  id?: number;
  fullName: string;
  email: string;
  phoneNumber: string;
  isActive: boolean;
}

const PlayerForm: React.FC = () => {
  const { id } = useParams<{ id?: string }>();
  const navigate = useNavigate();

  const [player, setPlayer] = useState<Player>({
    fullName: '',
    email: '',
    phoneNumber: '',
    isActive: true,
  });

  useEffect(() => {
    if (id) {
      axios
        .get<Player>(`/api/players/${id}`)
        .then(response => {
          setPlayer(response.data);
        })
        .catch(error => console.error('Error fetching player data:', error));
    }
  }, [id]);

  const handleChange = (event: React.ChangeEvent<unknown>) => {
    const target = event.target as HTMLInputElement | HTMLSelectElement;
    const { name, value } = target;

    setPlayer(prevState => ({
      ...prevState,
      [name]: name === 'isActive' ? value === 'true' : value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const apiUrl = id ? `/api/players/${id}` : '/api/players';
    const method = id ? 'put' : 'post';

    axios({
      method,
      url: apiUrl,
      data: player,
    })
      .then(() => {
        navigate('/players');
      })
      .catch(error => console.error('Error submitting player:', error));
  };

  return (
    <div>
      <h1>{id ? 'Edit Player' : 'Add New Player'}</h1>
      <Form onSubmit={handleSubmit}>
        <Row>
          <Col>
            <Form.Group controlId="formFullName">
              <Form.Label>Full Name</Form.Label>
              <Form.Control
                type="text"
                name="fullName"
                value={player.fullName}
                onChange={handleChange}
                required
              />
            </Form.Group>
          </Col>
        </Row>

        <Row>
          <Col>
            <Form.Group controlId="formEmail">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                name="email"
                value={player.email}
                onChange={handleChange}
                required
              />
            </Form.Group>
          </Col>
        </Row>

        <Row>
          <Col>
            <Form.Group controlId="formPhoneNumber">
              <Form.Label>Phone Number</Form.Label>
              <Form.Control
                type="text"
                name="phoneNumber"
                value={player.phoneNumber}
                onChange={handleChange}
                required
              />
            </Form.Group>
          </Col>
        </Row>

        <Row>
          <Col>
            <Form.Group controlId="formStatus">
              <Form.Label>Status</Form.Label>
              <Form.Control
                as="select"
                name="isActive"
                value={player.isActive ? 'true' : 'false'}
                onChange={handleChange}
              >
                <option value="true">Active</option>
                <option value="false">Inactive</option>
              </Form.Control>
            </Form.Group>
          </Col>
        </Row>

        <Button variant="primary" type="submit">
          {id ? 'Update Player' : 'Add Player'}
        </Button>
      </Form>
    </div>
  );
};

export default PlayerForm;
