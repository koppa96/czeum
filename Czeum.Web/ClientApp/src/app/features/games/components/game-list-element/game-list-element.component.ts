import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { GameState, GameTypeDto, MatchStatus } from '../../../../shared/clients';
import { faChessPawn, faCircle } from '@fortawesome/free-solid-svg-icons';
import { GameIconMapperService } from '../../services/game-icon-mapper.service';
import { IconDefinition } from '@fortawesome/fontawesome-common-types';
import { getLastOnlineText } from '../../../../shared/services/date-utils';

@Component({
  selector: 'app-game-list-element',
  templateUrl: './game-list-element.component.html',
  styleUrls: ['./game-list-element.component.scss']
})
export class GameListElementComponent implements OnInit {
  chessIcon = faChessPawn;
  connect4Icon = faCircle;

  @Input() data: MatchStatus;
  @Input() currentUserName;
  @Input() gameTypes: GameTypeDto[] = [];
  @Output() playClicked = new EventEmitter();

  constructor(private gameIconMapperService: GameIconMapperService) { }

  ngOnInit() {
    console.log(this.gameTypes);
  }

  getIcon(gameIdentifier: number): IconDefinition {
    return this.gameIconMapperService.mapIcon(gameIdentifier);
  }

  getGameTypeDisplayName(): string {
    const gameType = this.gameTypes.find(x => x.identifier === this.data.currentBoard.gameIdentifier);
    if (gameType) {
      return gameType.displayName;
    } else {
      return '';
    }
  }

  getLastMoveText(): string {
    return getLastOnlineText(this.data.lastMoveDate);
  }

  getCurrentPlayerText(): string {
    const currentPlayer = this.data.players.find(x => x.playerIndex === this.data.currentPlayerIndex);
    return currentPlayer.username === this.currentUserName ? 'Te következel' : `${currentPlayer.username} következik`;
  }

  getWinnerText(): string {
    if (this.data.winner === this.currentUserName) {
      return 'Te nyertél';
    } else if (this.data.winner) {
      return `${this.data.winner} nyert`;
    } else {
      return 'Döntetlen';
    }
  }

  isInProgress(): boolean {
    return this.data.state === GameState.YourTurn || this.data.state === GameState.EnemyTurn;
  }

  onPlayClicked() {
    this.playClicked.emit();
  }

}
