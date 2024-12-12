import React from "react";
import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";

type MemberCardProps = {
  name: string;
  email: string;
  status: string;
  id: string;
};

const MemberCard: React.FC<MemberCardProps> = ({ name, email, status, id }) => {
  return (
    <Card style={{ width: "18rem", margin: "10px" }}>
      <Card.Body>
        <Card.Title>{name}</Card.Title>
        <Card.Text>
          <strong>Email:</strong> {email}
          <br />
          <strong>Status:</strong> {status}
        </Card.Text>
        <Link to={`/member-detail/${id}`} className="btn btn-primary">
          View Details
        </Link>
      </Card.Body>
    </Card>
  );
};

export { MemberCard };
