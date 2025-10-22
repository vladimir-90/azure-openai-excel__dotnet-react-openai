interface SpinnerProps {
	sm?: boolean;
}

function Spinner({ sm = false }: SpinnerProps) {
	return (
		<div
			className={`spinner-border text-primary ${
				sm ? 'spinner-border-sm' : ''
			}`}
			role="status"
		>
			<span className="visually-hidden">Loading...</span>
		</div>
	);
}

export default Spinner;
