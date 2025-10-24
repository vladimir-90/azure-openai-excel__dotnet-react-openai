import type { OpenAIQueryCost } from '../../services/excel-analysis.service';

interface AIRequestCostProps {
	cost: OpenAIQueryCost | null;
	variant?: 'default' | 'compact';
}

function AIRequestCost({ cost, variant = 'default' }: AIRequestCostProps) {
	if (!cost) return null;

	if (variant === 'compact') {
		return (
			<span className="cost-badge-compact">
				💰 ${cost.totalCost.toFixed(6)} • {cost.inputTokenCount}→
				{cost.outputTokenCount}
				{cost.reasoningTokenCount > 0 && (
					<> ({cost.reasoningTokenCount})</>
				)}
			</span>
		);
	}

	return (
		<div className="mt-2">
			<span className="badge rounded-pill px-3 py-2 cost-badge-ai">
				💰 ${cost.totalCost} • {cost.inputTokenCount}→
				<span>{cost.outputTokenCount} </span>
				{cost.reasoningTokenCount > 0 && (
					<span>({cost.reasoningTokenCount}) </span>
				)}
				<span>tokens</span>
			</span>
		</div>
	);
}

export default AIRequestCost;
