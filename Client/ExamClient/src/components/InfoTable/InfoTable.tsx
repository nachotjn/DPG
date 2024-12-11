import React from 'react';
import { Table } from 'react-bootstrap';

// Definimos las props para la tabla, en este caso, para los detalles de la semana
interface InfoTableProps {
  headers: string[];
  data: Array<Record<string, string | number>>; // Aseguramos que los datos sean un array de objetos con valores de tipo string o number
}

const InfoTable: React.FC<InfoTableProps> = ({ headers, data }) => {
  return (
    <Table striped bordered hover>
      <thead>
        <tr>
          {headers.map((header, index) => (
            <th key={index}>{header}</th>
          ))}
        </tr>
      </thead>
      <tbody>
        {data.map((row, rowIndex) => (
          <tr key={rowIndex}>
            {Object.values(row).map((value, colIndex) => (
              <td key={colIndex}>{value}</td>
            ))}
          </tr>
        ))}
      </tbody>
    </Table>
  );
};

export default InfoTable;
